import { Component, Input, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { HttpConnection, HubConnection } from '@aspnet/signalr';
import { Observable, Subscriber, Subscription } from 'rxjs/';

import { AlertService } from '../../services/alert/alert.service';
import { Tournament, Position, Status } from '../../tournament';
import { Score } from '../../score';
import { MovementDescription } from '../../movement';
import { TournamentService, TOURNAMENT_SERVICE } from '../../services/tournament/tournament.service';
import { DealService, DEAL_SERVICE } from '../../services/deal/deal.service';
import { MovementService, MOVEMENT_SERVICE } from '../../services/movement/movement.service';

@Component({
    selector: 'lanfeust-bridge-tournament',
    templateUrl: './tournament.html'
})
export class TournamentComponent implements OnInit {
    @Input() id: number;

    _created = false;
    _edit = false;
    _tournament: Tournament;
    _knownMovements: Map<string, MovementDescription> = new Map<string, MovementDescription>();
    _sortedMovementIds: string[] = [];
    _knownScorings: string[] = [];
    _knownNames: string[] = [];
    _invalidReason: string;

    _currentRound = 0;
    _roundFinished = false;
    _currentDeal = 1;
    _currentPlayer = 0;
    _currentPosition: Position;
    _currentScore: Score = new Score;
    _scoreDisplayed = false;

    constructor(
        private _router: Router,
        private _route: ActivatedRoute,
        private _alertService: AlertService,
        @Inject(TOURNAMENT_SERVICE) private _tournamentService: TournamentService,
        @Inject(DEAL_SERVICE) private _dealService: DealService,
        @Inject(MOVEMENT_SERVICE) private _movementService: MovementService) { }

    ngOnInit() {
        this._movementService.getMovements().subscribe(movements => {
            movements.forEach(d => { this._knownMovements[d.id] = d; this._sortedMovementIds.push(d.id); });
            if (this._tournament && !this._tournament.movement && movements.length > 0) {
                this.movementId = movements[0].id;
            }
        });
        this._tournamentService.getScorings().subscribe(scorings => {
            this._knownScorings = scorings;
            if (this._tournament && !this._tournament.scoring && scorings.length > 0)
                this._tournament.scoring = scorings[0];
        });
        this._tournamentService.getNames()
            .subscribe(names => this._knownNames = names.filter(v => v !== undefined).map(v => v.name));

        this._route.url
            .switchMap((urlSegments: UrlSegment[]) => {
                if (urlSegments[0].path === 'new-tournament') {
                    const tournament = new Tournament;
                    tournament.nbTables = 1;
                    tournament.nbRounds = 1;
                    tournament.nbDealsPerRound = 2;
                    for (let i = 0; i < 4; i++)
                        tournament.players.push({name: 'Player ' + (i + 1), score: 0, rank: 0});
                    return Observable.of(tournament);
                }
                const id = +urlSegments[1].path;
                return this._tournamentService.get(id);
            })
            .subscribe((tournament: Tournament) => {
                this._tournament = tournament;
                if (tournament.id === 0) {
                    if (this._sortedMovementIds.length > 0)
                        this.movementId = this._sortedMovementIds[0];
                    if (this._knownScorings.length > 0)
                        this._tournament.scoring = this._knownScorings[0];
                    this._created = false;
                    this._edit = true;
                } else {
                    console.log('tournament service returned', tournament);
                    this._created = true;
                    this._edit = false;
                    this.movementId = tournament.movement;
                    if (tournament.status === Status.Running)
                        this.processRunning(tournament.id);
                }
            }, error => {
                console.log('Error loading tournament : ', error);
                this._alertService.newAlert.next({msg: error, type: 'warning', dismissible: true});
                this._router.navigate(['/']);
            });
        this._tournamentService.tournamentStartedObservable.subscribe(tournamentId => {
            if (this._tournament && this._tournament.id === tournamentId) {
                this._tournament.status = Status.Running;
                this.processRunning(tournamentId);
            }
        });
        this._tournamentService.tournamentFinishedObservable.subscribe(tournamentId => {
            if (this._tournament && this._tournament.id === tournamentId) {
                this._tournament.status = Status.Finished;
            }
        });
    }

    onSubmit() {
        // isValid if the button could be clicked (and checked on server side)
        if (this._created) {
           this._tournamentService.update(this._tournament)
                .subscribe(tournament => {
                    this._alertService.newAlert.next({
                        msg: 'Tournament \'' + tournament.name + '\' successfully updated',
                        type: 'success',
                        dismissible: true
                    });
                    this._tournament = tournament;
                }, reason => {
                    this._alertService.newAlert.next({
                        msg: 'Tournament update for \'' + this._tournament.name + '\' failed : ' + reason,
                        type: 'warning',
                        dismissible: true
                    });
                    this._edit = true;
                });
        } else {
           this._tournamentService.create(this._tournament)
               .subscribe(tournament => {
                   this._alertService.newAlert.next({
                       msg: 'Tournament \'' + tournament.name + '\' successfully created',
                       type: 'success',
                       dismissible: true
                   });
                   this._tournament = tournament;
                   this._created = true;
                   // since the route will get back to us (as a component), ngOnInit isn't necessarily called
                   // well, actually it *is* called...
                   this._router.navigate(['/tournament', tournament.id]);
               }, reason => {
                   this._alertService.newAlert.next({
                       msg: 'Tournament creation for \'' + this._tournament.name + '\' failed : ' + reason,
                       type: 'warning',
                       dismissible: true
                   });
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
            .subscribe(tournament => {
                this._alertService.newAlert.next({
                    msg: 'Tournament "' + tournament.name + '" started',
                    type: 'success',
                    dismissible: true
                });
                this._tournament = tournament;
            }, reason => {
                this._alertService.newAlert.next({
                    msg: 'Tournament "' + this._tournament.name + '" failed to start : ' + reason,
                    type: 'warning',
                    dismissible: true
                });
                // go back to setup status
                this._tournament.status = Status.Setup;
            });
    }

    close() {
        // tentatively set as finished
        this._tournament.status = Status.Finished;
        this._tournamentService.close(this._tournament.id)
            .subscribe(tournament => {
                this._alertService.newAlert.next({
                    msg: 'Tournament "' + tournament.name + '" finished',
                    type: 'success',
                    dismissible: true
                });
                this._tournament = tournament;
            }, reason => {
                this._alertService.newAlert.next({
                    msg: 'Tournament "' + this._tournament.name + '" failed to close : ' + reason,
                    type: 'warning',
                    dismissible: true
                });
                // go back to running status
                this._tournament.status = Status.Running;
            });
    }

    processRunning(tournamentId: number) {
        this._tournamentService.currentRound(tournamentId)
        .subscribe(r => {
            this._currentRound = r.round;
            this._roundFinished = r.finished;
            this.initializeScore(true);
        });
        this._tournamentService.getRoundFinishedObservable(tournamentId)
            .subscribe(r => {
                console.log('RoundFinished received', r);
                this._currentRound = r;
                this._roundFinished = true;
        });
        this._tournamentService.getNextRoundObservable(tournamentId)
            .subscribe(r => {
                console.log('NextRound received', r);
                this._currentRound = r;
                this._roundFinished = false;
                this.initializeScore(true);
        });
    }

    initializeScore(resetCurrentDeal: boolean) {
        this._scoreDisplayed = false;
        this._currentRound = +this._currentRound; // string if set from html
        if (this._currentRound >= this._tournament.nbRounds)
            return;
        this._currentPosition = this._tournament.positions[this._currentRound][this._currentPlayer];
        // only display score form for deals from round
        if (resetCurrentDeal)
            this._currentDeal = this.roundDeals[0];
        if (this.roundDeals.indexOf(this._currentDeal) === -1)
            return;
        this._dealService.getScore(this._tournament.id, this._currentDeal, this._currentRound)
            .subscribe(score => {
                score.players = {
                    north: this._tournament.players[this._currentPosition.north].name,
                    south: this._tournament.players[this._currentPosition.south].name,
                    east: this._tournament.players[this._currentPosition.east].name,
                    west: this._tournament.players[this._currentPosition.west].name
                };
                this._currentScore = score;
                this._scoreDisplayed = true;
            });
    }

    scoreValidated() {
        // TODO : set to first non-played deal of the round
        // need to subscribe so that the POST is actually sent
        this._dealService.postScore(this._tournament.id, this._currentScore)
            .subscribe(s => console.log('POST score returned', s));
        this._currentDeal++;
        this.initializeScore(false);
    }

    nextRound() {
        this._tournamentService.nextRound(this._tournament.id);
        // simply set _roundFinished for display, and wait notification of next round
        this._roundFinished = false;
    }

    get rounds(): number[] {
        if (this._tournament) {
            const rounds = [];
            for (let i = 0; i < this._tournament.nbRounds; i++)
                rounds.push(i);
            return rounds;
        } else {
            return [];
        }
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
        return this._tournament.status === Status.Setup;
    }

    get running() {
        return this._tournament.status === Status.Running;
    }

    get finished() {
        return this._tournament.status === Status.Finished;
    }

    get players() {
        const nbPlayers = this._tournament.nbTables * 4;
        if (this._tournament.players.length < nbPlayers)
            for (let i = this._tournament.players.length; i < nbPlayers; i++)
                this._tournament.players.push({name: 'Player ' + (i + 1), score: 0, rank: 0});
        return this._tournament.players.filter((p, i) => i < nbPlayers);
    }

    get sortedPlayers() {
        return this.players.sort((a, b) => a.rank - b.rank);
    }

    get nbTables() {
        return this._tournament ? this._tournament.nbTables : 1;
    }

    set nbTables(value) {
        if (!this._tournament)
            return;
        this._tournament.nbTables = value;
        const nbPlayers = this._tournament.nbTables * 4;
        if (this._tournament.players.length < nbPlayers)
            for (let i = this._tournament.players.length; i < nbPlayers; i++)
                this._tournament.players.push({ name: 'Player ' + (i + 1), score: 0, rank: 0 });
    }

    get minTables(): number {
        return this.movement ? this.movement.minTables : 1;
    }

    get maxTables(): number {
        return (this.movement && this.movement.maxTables !== -1) ? this.movement.maxTables : 100;
    }

    get fixedNbTables() {
        return this.movement && this.movement.minTables === this.movement.maxTables;
    }

    get nbRounds() {
        return this._tournament ? this._tournament.nbRounds : 1;
    }

    set nbRounds(value) {
        if (!this._tournament || value < this.movement.minRounds || (this.movement.maxRounds !== -1 && value > this.movement.maxRounds))
            return;
        this._tournament.nbRounds = value;
    }

    get fixedNbRounds() {
        return this.movement && this.movement.minRounds === this.movement.maxRounds;
    }

    get minRounds(): number {
        return this.movement ? this.movement.minRounds : 1;
    }

    get maxRounds(): number {
        return (this.movement && this.movement.maxRounds !== -1) ? this.movement.maxRounds : 100;
    }

    get movementId() {
        return this._tournament ? this._tournament.movement
            : (this._sortedMovementIds.length > 0 ? this._sortedMovementIds[0] : undefined);
    }

    set movementId(value) {
        if (!this._tournament || this._tournament.movement === value)
            return;
        this._tournament.movement = value;
        const movement: MovementDescription = this._knownMovements[value];
        this.nbTables = movement.minTables;
        this._tournament.nbRounds = movement.minRounds;
    }

    get movement(): MovementDescription {
        const id = this._tournament ? this._tournament.movement
            : (this._sortedMovementIds.length > 0 ? this._sortedMovementIds[0] : undefined);
        return this._knownMovements[id];
    }

    get movementDescription() {
        return this.movement ? this.movement.description : '';
    }

    get nbDeals() {
        return this._tournament.nbDeals;
    }

    get deals() {
        const result = [];
        for (let i = 0; i < this.nbDeals; i++)
            result.push({id: i + 1});
        return result;
    }

    get isValid() {
        if (!this._tournament.nbTables || this._tournament.nbTables < 1 || this._tournament.nbTables > 100) {
            this._invalidReason = 'Number of tables must be between 1 and 100';
            return false;
        }
        if (!this._tournament.nbRounds || this._tournament.nbRounds < 1 || this._tournament.nbRounds > 100) {
            this._invalidReason = 'Number of rounds must be between 1 and 100';
            return false;
        }
        if (!this._tournament.nbDealsPerRound || this._tournament.nbDealsPerRound < 1 ||
            this._tournament.nbDealsPerRound > 100) {
            this._invalidReason = 'Number of deals per round must be between 1 and 100';
            return false;
        }
        const nbPlayers = this._tournament.nbTables * 4;
        const emptyNames = this._tournament.players.filter((p, i) => i < nbPlayers && !p.name);
        if (emptyNames.length > 0) {
            this._invalidReason = 'All players must be filled';
            return false;
        }
        const distinctPlayers = new Set(this._tournament.players.filter((p, i) => i < nbPlayers).map(p => p.name));
        if (distinctPlayers.size !== nbPlayers) {
            this._invalidReason = 'All players must have distinct names';
            return false;
        }
        if (!this._created && this._knownNames.indexOf(this._tournament.name) !== -1) {
            this._invalidReason = 'Name already exists';
            return false;
        }
        return true;
    }

    get diagnostic() { return this._tournament ? JSON.stringify(this._tournament) : 'undefined'; }
}