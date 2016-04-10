export class Deal {
    id: number = 1;
    dealer: string = 'N';
    vulnerability: string = 'None';
    scores: [
        {
            contract: string;
        }
    ];
}