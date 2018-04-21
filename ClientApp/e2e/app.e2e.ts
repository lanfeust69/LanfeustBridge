import { browser, by, element, ExpectedConditions } from 'protractor';

import { parse as parseUrl } from 'url';

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
            const el = element(by.css('div[name="result"]>button:nth-of-type(' + (result < 0 ? 1 : 3) + ')'));
            for (let i = 0; i < Math.abs(result); i++)
                el.click();
        }
    }
    element(by.buttonText('Accept')).click();
}

describe('front-end App', function() {
    it('should start on login page', () => {
        browser.waitForAngularEnabled(false);
        browser.get('/');
        browser.wait(ExpectedConditions.urlContains('/Identity/Account/Login'), 12000);
        expect(element(by.id('account'))).toBeTruthy(); // the account form is present
        expect(element(by.css('form#account'))).toBeTruthy(); // the account form is present
        const registerLink = element(by.css('form#account p:nth-of-type(2) a'));
        expect(registerLink).toBeTruthy();
        expect(registerLink.getText()).toBe('Register as a new user');
        registerLink.click();
        browser.wait(ExpectedConditions.urlContains('/Identity/Account/Register'), 12000);
    });

    it('should run a small tournament', () => {
        browser.waitForAngularEnabled(false);
        browser.get('/');
        browser.wait(ExpectedConditions.urlContains('/Identity/Account/Login'), 12000);
        const registerLink = element(by.css('form#account p:nth-of-type(2) a'));
        registerLink.click();
        browser.wait(ExpectedConditions.urlContains('/Identity/Account/Register'), 12000);

        // register user
        element(by.id('Input_Email')).sendKeys('test@example.com');
        element(by.id('Input_Password')).sendKeys('123456');
        element(by.id('Input_ConfirmPassword')).sendKeys('123456');
        element(by.css('form button')).click();
        browser.wait(ExpectedConditions.not(ExpectedConditions.urlContains('/Identity')), 12000);
        expect(browser.getCurrentUrl().then(s => parseUrl(s).pathname)).toBe('/');
        browser.waitForAngularEnabled(true);

        element(by.css('button')).click();
        element(by.css('input[name="name"]')).sendKeys('Test');
        element(by.css('input[name="tables"]')).clear();
        element(by.css('input[name="tables"]')).sendKeys('3');
        element(by.css('input[name="rounds"]')).clear();
        element(by.css('input[name="rounds"]')).sendKeys('3');
        // fill in players
        element(by.linkText('Players')).click();
        for (let i = 1; i <= 12; i++)
            element(by.css(`form div:nth-of-type(${i}) input`)).sendKeys(`Player ${i}`);
        element(by.linkText('Infos')).click();

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

        expect(element(by.linkText('Play'))).toBeTruthy();
        expect(element(by.linkText('Players'))).toBeTruthy();
        element(by.linkText('Players')).click();
        element(by.linkText('Player 1')).click();
        element(by.linkText('2')).click();
        element(by.linkText('Previous')).click();
        // browser.pause();
        // browser.manage().logs().get('browser').then(console.log);
    });
});
