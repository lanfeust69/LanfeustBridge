import { browser, by, element } from 'protractor';

export class FrontEndPage {
  navigateTo() {
    return browser.get('/');
  }

  getMainTitleText() {
    return element(by.css('lanfeust-bridge-app h1')).getText();
  }
}
