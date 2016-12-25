import {Inject, Injectable} from '@angular/core';
import {Tournament, Position, Status} from '../../tournament';
import {Score} from '../../score';
import {DealService, DEAL_SERVICE} from '../deal/deal.service';
import {TournamentService} from './tournament.service';

@Injectable()
export class TournamentServiceMock implements TournamentService {
    private _tournaments: Tournament[] = [];

    constructor(@Inject(DEAL_SERVICE) private _dealService: DealService) {}

    getNames() : Promise<{id: number; name: string}[]> {
        let result = this._tournaments.map((value, i) => ({id: i, name: value.name}));
        return Promise.resolve(result);
    }

    get(id: number) : Promise<Tournament> {
        if (id < 0 || id >= this._tournaments.length || !this._tournaments[id]) 
            return Promise.reject<Tournament>("No tournament with id '" + id + "' found");
        let tournament = this._tournaments[id];
        //return Promise.resolve(tournament);
        return new Promise<Tournament>(resolve => setTimeout(() => resolve(tournament), 2000)); // 2 seconds
    }

    create(tournament: Tournament) : Promise<Tournament> {
        tournament.id = this._tournaments.length;
        let allPositions: Position[][] = [];
        // mock : mitchell
        for (let r = 0; r < tournament.nbRounds; r++) {
            let positions: Position[] = [];
            for (let t = 0; t < tournament.nbTables; t++) {
                for (let i = 0; i < 4; i++) {
                    let position = new Position;
                    position.table = t + 1;
                    position.north = t * 4;
                    position.south = t * 4 + 1;
                    position.east = t * 4 + 2;
                    position.west = t * 4 + 3;
                    position.deals = [];
                    for (let d = 0; d < tournament.nbDealsPerRound; d++) {
                        position.deals.push(((t + r) * tournament.nbDealsPerRound) % (tournament.nbTables * tournament.nbDealsPerRound) + d + 1);
                    }
                    positions.push(position);
                }
            }
            allPositions.push(positions);
        }
        tournament.positions = allPositions;
        // not always true, but OK for a mock
        tournament.nbDeals = tournament.nbDealsPerRound * tournament.nbRounds;
        this._tournaments.push(tournament);
        //return Promise.resolve(tournament);
        return new Promise<Tournament>(resolve => setTimeout(() => resolve(tournament), 400)); // 0.4 seconds
    }

    update(tournament: Tournament) : Promise<Tournament> {
        let id = tournament.id;
        if (id < 0 || id >= this._tournaments.length || !this._tournaments[id]) 
            return Promise.reject<Tournament>("No tournament with id '" + id + "' found");
        this._tournaments[id] = tournament;
        return Promise.resolve(tournament);
    }

    delete(id: number) : Promise<boolean> {
        if (id >= 0 && id < this._tournaments.length && this._tournaments[id]) {
            this._tournaments[id] = undefined;
            return Promise.resolve(true);
        }
        return Promise.resolve(false);
    }

    getMovements() : Promise<{name: string, nbTables: number}[]> {
        return Promise.resolve([{name: "Mitchell", nbTables: -1}, {name: "Teams", nbTables: 2}, {name: "Individual", nbTables: 3}]);
    }

    getScorings() : Promise<string[]> {
        return Promise.resolve(["Matchpoint", "IMP"]);
    }

    start(id: number) : Promise<Tournament> {
        if (id < 0 || id >= this._tournaments.length || !this._tournaments[id]) 
            return Promise.reject<Tournament>("No tournament with id '" + id + "' found");
        let tournament = this._tournaments[id];
        tournament.status = Status.Running;
        tournament.currentRound = 0;
        return Promise.resolve(tournament);        
    }

    close(id: number) : Promise<Tournament> {
        if (id < 0 || id >= this._tournaments.length || !this._tournaments[id]) 
            return Promise.reject<Tournament>("No tournament with id '" + id + "' found");
        let tournament = this._tournaments[id];
        tournament.status = Status.Finished;
        return Promise.resolve(tournament);
    }

    currentRound(id: number) : Promise<{round: number, finished: boolean}> {
        if (id < 0 || id >= this._tournaments.length || !this._tournaments[id]) 
            return Promise.reject<{round: number, finished: boolean}>("No tournament with id '" + id + "' found");
        let tournament = this._tournaments[id];
        // finished, waiting for close
        if (tournament.currentRound == tournament.nbRounds)
            return Promise.resolve({round: tournament.currentRound, finished: true});
        // only in mock : assume all deals are played each round
        let promises: Promise<Score>[] = [];
        for (let dealId = 1; dealId <= tournament.nbDealsPerRound * tournament.nbTables; dealId++) {
            promises.push(this._dealService.getScore(id, dealId, tournament.currentRound));
        }
        return Promise.all<Score>(promises).then(scores => {
            let finished = scores.every(s => s.entered);
            if (finished) console.log("scores : ", scores);
            return {round: tournament.currentRound, finished: finished};
        });
    }

    nextRound(id: number) {
        if (id < 0 || id >= this._tournaments.length || !this._tournaments[id]) 
            return Promise.reject("No tournament with id '" + id + "' found");
        let tournament = this._tournaments[id];
        if (tournament.currentRound < tournament.nbRounds)
            tournament.currentRound++;
    }
}
