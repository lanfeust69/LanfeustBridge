import {Component, Input, ViewChild} from 'angular2/core';
import {Deal} from './deal';
import {HandComponent} from './hand.component';

@Component({
    selector: 'deal',
    templateUrl: 'app/deal.html',
    directives: [HandComponent],
    styles: ['canvas { background-color: limeGreen; margin: 10px }']
})
export class DealComponent {
    deal: Deal = new Deal;
    @ViewChild("table") tableCanvas;
    ngAfterViewInit() {
        var context = this.tableCanvas.nativeElement.getContext("2d");
        var size = this.tableCanvas.nativeElement.height;
        var font = (size / 6) + "px arial";
        context.font = font;
        context.textBaseline="middle";
        context.textAlign="center";
        context.fillText("#1", 40, 40);
        for (var i = 0; i < 4; i++) {
            var x = [0, size / 3, size * 4 / 5, size / 3][i];
            var y = [size / 3, 0, size / 3, size * 4 / 5][i];
            var width = [size / 5, size / 3][i % 2];
            var height = [size / 3, size / 5][i % 2];
            var color = { None: ["white", "white"], Both: ["red", "red"], NS: ["white", "red"], EW: ["red", "white"] }[this.deal.vulnerability][i % 2];
            context.fillStyle = color;
            context.fillRect(x, y, width, height);
            context.strokeStyle = "black";
            context.strokeRect(x, y, width, height);
            var name = ["W", "N", "E", "S"][i];
            var textX = [size / 10, size / 2, size * 9 / 10, size / 2][i];
            var textY = [size / 2, size / 10, size / 2, size * 9 / 10][i];
            context.fillStyle = "black";
            if (name == this.deal.dealer) {
                name = "D";
                context.font = "bold " + font;
            } else {
                context.font = font;
            }
            context.fillText(name, textX, textY);
        }
    }
}
