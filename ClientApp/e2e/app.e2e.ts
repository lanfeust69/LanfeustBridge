import { browser, by, element, ExpectedConditions } from 'protractor';

import { FrontEndPage } from './app.po';

function randomScore() {
    const level = Math.floor((Math.random() * 8) + 1);
    element(by.css('div[name="level"]>label:nth-of-type(' + level + ')')).click();
    if (level > 1) {
        const suit = Math.floor((Math.random() * 5) + 1);
        element(by.css('div[name="suit"]>label:nth-of-type(' + suit + ')>lanfeust-bridge-suit>span:nth-of-type(1)>span')).click();
        if (Math.random() < 0.25) {
            if (Math.random() < 0.2) {
                element(by.css('label[name="redoubled"]')).click();
            } else {
                element(by.css('label[name="doubled"]')).click();
            }
        }
        const declarer = Math.floor((Math.random() * 4) + 1);
        element(by.css('div[name="declarer"]>label:nth-of-type(' + declarer + ')')).click();
        const result = Math.floor((Math.random() * 5) - 2);
        if (result !== 0) {
            const el = element(by.css('div[name="result"]>label:nth-of-type(' + (result < 0 ? 1 : 3) + ')'));
            for (let i = 0; i < Math.abs(result); i++)
                el.click();
        }
    }
    element(by.buttonText('Accept')).click();
}

describe('front-end App', function() {
    let page: FrontEndPage;

    beforeEach(() => {
        page = new FrontEndPage();
    });

    it('should display app name', () => {
        page.navigateTo();
        expect<any>(page.getMainTitleText()).toEqual('Lanfeust Bridge');
    });

    it('should run a small tournament', () => {
        page.navigateTo();

        // browser.pause();
        element(by.css('button')).click();
        element(by.css('input[name="name"]')).sendKeys('Test');
        element(by.css('input[name="tables"]')).clear();
        element(by.css('input[name="tables"]')).sendKeys('3');
        element(by.css('input[name="rounds"]')).clear();
        element(by.css('input[name="rounds"]')).sendKeys('3');
        element(by.buttonText('Create')).click();
        // apparently the new angular 2 HttpModule's Observable aren't waited by protractor, so :
        browser.wait(ExpectedConditions.elementToBeClickable(element(by.buttonText('Start'))), 12000);
        element(by.buttonText('Start')).click();
        // polling for scores is done outside angular, so we can keep synchronization
        // but after that, without synchronization, we should wait for the buttons...
        element(by.linkText('Play')).click();

        for (let round = 0; round < 3; round++) {
            // players 10, 11, 12 also contain 'Player 1'
            element.all(by.css('select[name="currentPlayer"]>option')).first().click();
            randomScore();
            randomScore();

            element(by.cssContainingText('select[name="currentPlayer"]>option', 'Player 5')).click();
            randomScore();
            randomScore();

            element(by.cssContainingText('select[name="currentPlayer"]>option', 'Player 9')).click();
            randomScore();
            randomScore();

            if (round < 2) {
                // wait for Next Round button
                browser.wait(ExpectedConditions.elementToBeClickable(element(by.buttonText('Next Round'))), 12000);
                element(by.buttonText('Next Round')).click();
                // then wait for next round to actually begin
                const roundSummary = element(by.cssContainingText('h4', 'Round'));
                browser.wait(ExpectedConditions.textToBePresentInElement(roundSummary, 'Round ' + (round + 2)), 12000);
            }
        }

        browser.wait(ExpectedConditions.presenceOf(element(by.buttonText('Close'))), 12000);
        element(by.buttonText('Close')).click();

        ExpectedConditions.presenceOf(element(by.linkText('Play')));
        ExpectedConditions.presenceOf(element(by.linkText('Players')));
        element(by.linkText('Players')).click();
        element(by.linkText('Player 1')).click();
        element(by.linkText('2')).click();
        element(by.linkText('Previous')).click();
        // browser.pause();
        // browser.manage().logs().get('browser').then(console.log);
    });
});