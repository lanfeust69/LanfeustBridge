import { chromium, FullConfig } from '@playwright/test';

async function globalSetup(config: FullConfig) {
    const browser = await chromium.launch();
    const context = await browser.newContext({ ignoreHTTPSErrors: true });
    const page = await context.newPage();
    await page.goto('https://localhost:5001/Identity/Account/Register?returnUrl=%2F');
    await page.fill('input[name="Input.Email"]', 'user@localhost');
    await page.fill('input[name="Input.DisplayName"]', 'User');
    await page.fill('input[name="Input.Password"]', 'pwd');
    await page.fill('input[name="Input.ConfirmPassword"]', 'pwd');

    await Promise.all([
        page.waitForNavigation(/*{ url: 'http://localhost:4200/' }*/),
        page.click('button:has-text("Register")')
    ]);
    await page.context().storageState({ path: 'storageState.json' });
    await browser.close();
}
  
export default globalSetup;
