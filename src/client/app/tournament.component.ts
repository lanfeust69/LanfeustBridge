import {Component, Input, Inject} from 'angular2/core';
import {Control} from 'angular2/common';
import {RouteParams, ROUTER_DIRECTIVES} from 'angular2/router';
import {Tournament} from './tournament';
import {TournamentService, TOURNAMENT_SERVICE} from './tournament.service';
import {Deal} from './deal';
import {DealService, DEAL_SERVICE} from './deal.service';
import {DealComponent} from './deal.component';

@Component({
    selector: 'tournament',
    templateUrl: 'app/tournament.html',
    directives: [ROUTER_DIRECTIVES, DealComponent]
})
export class TournamentComponent {
    @Input() name: string;

    _created: boolean = false;
    _edit: boolean = false;
    _tournament: Tournament;
    _knownMovements: string[] = [];
    _knownNames: string[] = [];
    _invalidReason: string;

    constructor(
        private _routeParams: RouteParams,
        @Inject(TOURNAMENT_SERVICE) private _tournamentService: TournamentService,
        @Inject(DEAL_SERVICE) private _dealService: DealService) {
            console.log("TournamentComponent constructor");
    }

    ngOnInit() {
        this.name = this._routeParams.get('name');
        console.log("name is " + this.name);
        this._tournamentService.getMovements().then(movements => {
            this._knownMovements = movements;
            if (this._tournament && !this._tournament.movement && movements.length > 0)
                this._tournament.movement = movements[0];
        });
        this._tournamentService.getNames().then(names => this._knownNames = names);
        this._tournamentService.get(this.name)
            .then(tournament => {
                console.log("tournament service returned", tournament);
                this._created = true;
                this._edit = false;
                this._tournament = tournament;
            })
            .catch(reason => {
                console.log("no tournament found, create a new one");
                this._tournament = new Tournament(this.name);
                this._tournament.nbTables = 1;
                this._tournament.nbRounds = 1;
                if (this._knownMovements.length > 0)
                    this._tournament.movement = this._knownMovements[0];
                this._created = false;
                this._edit = true;
            });
    }

    onSubmit() {
        // isValid if the button could be clicked (and checked on server side)
        if (this._created)
           this._tournamentService.update(this._tournament);
        else
           this._tournamentService.create(this._tournament);
        this._created = true;
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
        if (!this._created && this._knownNames.indexOf(this._tournament.name) != -1) {
            this._invalidReason = "Name already exists";
            return false;
        }
        return true;
    }

    get diagnostic() { return this._tournament ? JSON.stringify(this._tournament) : "undefined"; }
}
