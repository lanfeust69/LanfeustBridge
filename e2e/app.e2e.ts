import { browser, by, element } from 'protractor';
import { FrontEndPage } from './app.po';

describe('front-end App', function() {
  let page: FrontEndPage;

  beforeEach(() => {
    page = new FrontEndPage();
  })

  it('should display app name', () => {
    page.navigateTo();
    expect(page.getMainTitleText()).toEqual('Lanfeust Bridge');
  });

  it('should run a small tournament', () => {
    page.navigateTo();

    //browser.pause();
    element(by.css('button')).click();
    element(by.css('input[name="name"]')).click();
    element(by.css('input[name="name"]')).sendKeys('Test');
    element(by.buttonText('Create')).click();
    element(by.buttonText('Start')).click();
    // now that polling for scores has started, won't synchronize
    browser.ignoreSynchronization = true;
    element(by.linkText('Play')).click();
    element(by.css('fieldset[name="level"]>label:nth-of-type(4)')).click();
    element(by.css('fieldset[name="suit"]>label:nth-of-type(5)>suit>span:nth-of-type(1)')).click();
    element(by.css('fieldset[name="declarer"]>label:nth-of-type(2)')).click();
    element(by.buttonText('Accept')).click();

    element(by.css('fieldset[name="level"]>label:nth-of-type(3)')).click();
    element(by.css('fieldset[name="suit"]>label:nth-of-type(3)>suit>span:nth-of-type(1)>span')).click();
    element(by.css('fieldset[name="declarer"]>label:nth-of-type(3)')).click();
    element(by.css('fieldset[name="result"]>label:nth-of-type(3)')).click();
    element(by.css('fieldset[name="result"]>label:nth-of-type(3)')).click();
    element(by.buttonText('Accept')).click();
    browser.sleep(5000);
    element(by.buttonText('Close')).click();

    browser.ignoreSynchronization = false;
    element(by.linkText('Play')).isPresent()
    element(by.linkText('Players')).isPresent()
    element(by.linkText('Players')).click()
    element(by.linkText('Player 1')).click()
    element(by.linkText('2')).click()
    element(by.linkText('Previous')).click()
  })
});
