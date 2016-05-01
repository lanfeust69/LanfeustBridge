import {Component, Input, Inject} from 'angular2/core';
import {Control} from 'angular2/common';
import {RouteParams, Router, ROUTER_DIRECTIVES} from 'angular2/router';
import {TAB_DIRECTIVES} from 'ng2-bootstrap/ng2-bootstrap';
import {AlertService} from './alert.service';
import {Tournament, Player} from './tournament';
import {TournamentService, TOURNAMENT_SERVICE} from './tournament.service';
import {Deal} from './deal';
import {DealService, DEAL_SERVICE} from './deal.service';
import {DealComponent} from './deal.component';

@Component({
    selector: 'tournament',
    templateUrl: 'app/tournament.html',
    directives: [ROUTER_DIRECTIVES, TAB_DIRECTIVES, DealComponent]
})
export class TournamentComponent {
    @Input() id: number;

    _created: boolean = false;
    _edit: boolean = false;
    _tournament: Tournament;
    _knownMovements: string[] = [];
    _knownNames: string[] = [];
    _invalidReason: string;

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
        this._tournamentService.getNames().then(names => this._knownNames = names.filter(v => v != undefined).map(v => v.name));
        if (id == -1) {
            // called by NewTournament
            this._tournament = new Tournament;
            this._tournament.nbTables = 1;
            this._tournament.nbRounds = 1;
            for (let i = 0; i < 4; i++)
                this._tournament.players.push({name: "Player " + (i + 1), score: 0, rank: 0});
            if (this._knownMovements.length > 0)
                this._tournament.movement = this._knownMovements[0];
            this._tournament.deals = [new Deal(1)];
            this._created = false;
            this._edit = true;
        } else {
            this._tournamentService.get(id)
                .then(tournament => {
                    console.log("tournament service returned", tournament);
                    this._created = true;
                    this._edit = false;
                    this._tournament = tournament;
                })
                .catch(reason => {
                    console.log("no tournament found with id " + id);
                    this._alertService.newAlert.next({msg: "No tournament found with id " + id, type: 'warning', dismissible: true});
                    this._router.navigate(['TournamentList']);
                });
        }
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

    // apparently won't work with template-driven forms... 
    static nameValid(c: Control) {
        console.log("nameValid called");
        if (!c.value)
            return { reason: "required" };
        // if (/* cannot access instance field _knownNames here... */)
        //     return { reason: "already taken" };
        return null;
    }

    get players() {
        let nbPlayers = this._tournament.nbTables * 4;
        if (this._tournament.players.length < nbPlayers)
            for (let i = this._tournament.players.length; i < nbPlayers; i++)
                this._tournament.players.push({name: "Player " + (i + 1), score: 0, rank: 0});
        return this._tournament.players.filter((p, i) => i < nbPlayers);
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
