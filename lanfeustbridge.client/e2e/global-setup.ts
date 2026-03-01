import { test } from '@playwright/test';

test('Perform login', async ({ page }) => {
    await page.goto('https://localhost:54145/Identity/Account/Register?returnUrl=%2F');
    await page.fill('input[name="Input.Email"]', 'user@localhost');
    await page.fill('input[name="Input.DisplayName"]', 'User');
    await page.fill('input[name="Input.Password"]', 'pwd');
    await page.fill('input[name="Input.ConfirmPassword"]', 'pwd');

    await Promise.all([
        page.waitForNavigation(/*{ url: 'http://localhost:('54145')/' }*/),
        page.click('button:has-text("Register")')
    ]);
    await page.context().storageState({ path: 'e2e/storageState.json' });
});
