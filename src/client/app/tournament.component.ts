import {Component, Input, Inject} from 'angular2/core';
import {Control} from 'angular2/common';
import {RouteParams, Router, ROUTER_DIRECTIVES} from 'angular2/router';
import {TAB_DIRECTIVES} from 'ng2-bootstrap/ng2-bootstrap';
import {AlertService} from './alert.service';
import {Tournament, Player, Position, Status} from './tournament';
import {TournamentService, TOURNAMENT_SERVICE} from './tournament.service';
import {Score} from './score';
import {ScoreFormComponent} from './score-form.component';
import {Deal} from './deal';
import {DealService, DEAL_SERVICE} from './deal.service';
import {DealComponent} from './deal.component';

@Component({
    selector: 'tournament',
    templateUrl: 'app/tournament.html',
    directives: [ROUTER_DIRECTIVES, TAB_DIRECTIVES, DealComponent, ScoreFormComponent]
})
export class TournamentComponent {
    @Input() id: number;

    _created: boolean = false;
    _edit: boolean = false;
    _tournament: Tournament;
    _knownMovements: string[] = [];
    _knownScorings: string[] = [];
    _knownNames: string[] = [];
    _invalidReason: string;
    
    _currentRound: number = 0;
    _roundFinished: boolean = false;
    _polling: boolean = false;
    _stopPolling: boolean = false;
    _currentDeal: number = 1;
    _currentPlayer: number = 0;
    _currentPosition: Position;
    _currentScore: Score = new Score;
    _scoreDisplayed = false;

    constructor(
        private _router: Router,
        private _routeParams: RouteParams,
        private _alertService: AlertService,
        @Inject(TOURNAMENT_SERVICE) private _tournamentService: TournamentService,
        @Inject(DEAL_SERVICE) private _dealService: DealService) {}

    ngOnInit() {
        let id = +this._routeParams.get('id');
        console.log("id is " + id);
        this._tournamentService.getMovements().then(movements => {
            this._knownMovements = movements;
            if (this._tournament && !this._tournament.movement && movements.length > 0)
                this._tournament.movement = movements[0];
        });
        this._tournamentService.getScorings().then(scorings => {
            this._knownScorings = scorings;
            if (this._tournament && !this._tournament.scoring && scorings.length > 0)
                this._tournament.scoring = scorings[0];
        });
        this._tournamentService.getNames().then(names => this._knownNames = names.filter(v => v != undefined).map(v => v.name));
        if (id == -1) {
            // called by NewTournament
            this._tournament = new Tournament;
            this._tournament.nbTables = 1;
            this._tournament.nbRounds = 1;
            this._tournament.nbDealsPerRound = 2;
            for (let i = 0; i < 4; i++)
                this._tournament.players.push({name: "Player " + (i + 1), score: 0, rank: 0});
            if (this._knownMovements.length > 0)
                this._tournament.movement = this._knownMovements[0];
            if (this._knownScorings.length > 0)
                this._tournament.scoring = this._knownScorings[0];
            this._created = false;
            this._edit = true;
        } else {
            this._tournamentService.get(id)
                .then(tournament => {
                    console.log("tournament service returned", tournament);
                    this._created = true;
                    this._edit = false;
                    this._tournament = tournament;
                    if (tournament.status == Status.Running)
                        this._tournamentService.currentRound(tournament.id)
                            .then(r => {
                                this._currentRound = r.round;
                                this._roundFinished = r.finished;
                                this.initializeScore(true);
                                if (!this._polling)
                                    this.pollEndOfRound(this);
                            });
                })
                .catch(reason => {
                    console.log("no tournament found with id " + id);
                    this._alertService.newAlert.next({msg: "No tournament found with id " + id, type: 'warning', dismissible: true});
                    this._router.navigate(['TournamentList']);
                });
        }
    }

    ngOnDestroy() {
        this._stopPolling = true;
    }

    onSubmit() {
        // isValid if the button could be clicked (and checked on server side)
        if (this._created) {
           this._tournamentService.update(this._tournament);
        } else {
           this._tournamentService.create(this._tournament)
               .then(tournament => {
                   this._alertService.newAlert.next({msg: "Tournament '" + tournament.name + "' successfully created", type: 'success', dismissible: true});
                   this._tournament = tournament;
                   this._created = true;
               }).catch(reason => {
                   this._alertService.newAlert.next({msg: "Tournament creation for '" + this._tournament.name + "' failed : " + reason, type: 'warning', dismissible: true});
                   this._edit = true;
               });
        }
        this._edit = false;
    }

    start() {
        // tentatively set as running
        this._tournament.status = Status.Running;
        this._currentRound = 0;
        this._currentDeal = 1;
        this._currentPlayer = 0;
        this.initializeScore(true);
        this._tournamentService.start(this._tournament.id)
            .then(tournament => {
                this._alertService.newAlert.next({msg: "Tournament '" + tournament.name + "' started", type: 'success', dismissible: true});
                this._tournament = tournament;
                if (!this._polling)
                    this.pollEndOfRound(this);
            }).catch(reason => {
                this._alertService.newAlert.next({msg: "Tournament '" + this._tournament.name + "' failed to start : " + reason, type: 'warning', dismissible: true});
                // go back to setup status
                this._tournament.status = Status.Setup;
            });
    }

    pollEndOfRound(self: TournamentComponent) {
        self._polling = true;
        if (self._stopPolling)
            return;
        self._tournamentService.currentRound(self._tournament.id)
            .then(r => {
                self._roundFinished = r.finished;
                if (r.round != self._currentRound) {
                    self._currentRound = r.round;
                    self.initializeScore(true);
                }
                if (self._currentRound < self._tournament.nbRounds)
                    setTimeout(self.pollEndOfRound, 5000, self);
                else
                    self._polling = false;
            });
    }

    close() {
        // tentatively set as finished
        this._tournament.status = Status.Finished;
        this._tournamentService.close(this._tournament.id)
            .then(tournament => {
                this._alertService.newAlert.next({msg: "Tournament '" + tournament.name + "' finished", type: 'success', dismissible: true});
                this._tournament = tournament;
            }).catch(reason => {
                this._alertService.newAlert.next({msg: "Tournament '" + this._tournament.name + "' failed to close : " + reason, type: 'warning', dismissible: true});
                // go back to running status
                this._tournament.status = Status.Running;
            });
    }

    initializeScore(resetCurrentDeal: boolean) {
        this._scoreDisplayed = false;
        if (this._currentRound >= this._tournament.nbRounds)
            return;
        this._currentPosition = this._tournament.positions[this._currentRound][this._currentPlayer];
        // only display score form for deals from round
        if (resetCurrentDeal)
            this._currentDeal = this.roundDeals[0];
        if (this.roundDeals.indexOf(this._currentDeal) == -1)
            return;
        this._dealService.getScore(this._tournament.id, this._currentDeal, this._currentRound)
            .then(score => {
                score.players = {
                    north: this._tournament.players[this._currentPosition.north].name,
                    south: this._tournament.players[this._currentPosition.south].name,
                    east: this._tournament.players[this._currentPosition.east].name,
                    west: this._tournament.players[this._currentPosition.west].name
                };
                this._currentScore = score;
                this._scoreDisplayed = true
            });
    }

    scoreValidated() {
        console.log("scoreValidated");
        // TODO : set to first non-played deal of the round
        this._dealService.postScore(this._tournament.id, this._currentScore);
        this._currentDeal++;
        this.initializeScore(false);
    }

    nextRound() {
        this._tournamentService.nextRound(this._tournament.id);
        // simply set _roundFinished for display, and wait for pollEndOfRound
        this._roundFinished = false;
    }

    get roundFinished() {
        return this._roundFinished;
    }

    get roundDeals() {
        return this._currentPosition.deals;
    }

    get currentDeal(): number {
        return this._currentDeal;
    }

    // handle the list selection setting to a string
    set currentDeal(value) {
        this._currentDeal = +value;
    }

    get setup() {
        return this._tournament.status == Status.Setup;
    }

    get running() {
        return this._tournament.status == Status.Running;
    }

    get finished() {
        return this._tournament.status == Status.Finished;
    }

    get players() {
        let nbPlayers = this._tournament.nbTables * 4;
        if (this._tournament.players.length < nbPlayers)
            for (let i = this._tournament.players.length; i < nbPlayers; i++)
                this._tournament.players.push({name: "Player " + (i + 1), score: 0, rank: 0});
        return this._tournament.players.filter((p, i) => i < nbPlayers);
    }

    get nbDeals() {
        return this._tournament.nbRounds * this._tournament.nbDealsPerRound;
    }

    get deals() {
        let result = [];
        for (let i = 0; i < this.nbDeals; i++)
            result.push({id: i + 1});
        return result;
    }

    get isValid() {
        //console.log("isValid called");
        if (!this._tournament.nbTables || this._tournament.nbTables < 1 || this._tournament.nbTables > 100) {
            this._invalidReason = "Number of tables must be between 1 and 100";
            return false;
        }
        if (!this._tournament.nbRounds || this._tournament.nbRounds < 1 || this._tournament.nbRounds > 100) {
            this._invalidReason = "Number of rounds must be between 1 and 100";
            return false;
        }
        if (!this._tournament.nbDealsPerRound || this._tournament.nbDealsPerRound < 1 || this._tournament.nbDealsPerRound > 100) {
            this._invalidReason = "Number of deals per round must be between 1 and 100";
            return false;
        }
        let nbPlayers = this._tournament.nbTables * 4;
        let emptyNames = this._tournament.players.filter((p, i) => i < nbPlayers && !p.name);
        if (emptyNames.length > 0) {
            this._invalidReason = "All players must be filled";
            return false;
        }
        let distinctPlayers = new Set(this._tournament.players.filter((p, i) => i < nbPlayers).map(p => p.name));
        if (distinctPlayers.size != nbPlayers) {
            this._invalidReason = "All players must have distinct names";
            return false;
        }
        if (!this._created && this._knownNames.indexOf(this._tournament.name) != -1) {
            this._invalidReason = "Name already exists";
            return false;
        }
        return true;
    }

    get diagnostic() { return this._tournament ? JSON.stringify(this._tournament) : "undefined"; }
}
