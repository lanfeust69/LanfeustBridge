import {OpaqueToken} from 'angular2/core';
import {Tournament} from './tournament';

export let TOURNAMENT_SERVICE = new OpaqueToken('TournamentService');

export interface TournamentService {
    getTournament(tournamentName: string) : Promise<Tournament>;
    createTournament(tournament: Tournament) : Promise<Tournament>;
    updateTournament(tournament: Tournament) : Promise<Tournament>;
}
