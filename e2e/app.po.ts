export class FrontEndPage {
  navigateTo() {
    return browser.get('/');
  }

  getParagraphText() {
    return element(by.css('front-end-app p')).getText();
  }
}
