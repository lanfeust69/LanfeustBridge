import { test, expect, Page } from '@playwright/test';

async function randomScore(page: Page) {
    const level = Math.floor((Math.random() * 8) + 1);
    await page.click('div[name="level"]>label:nth-of-type(' + level + ')');
    if (level > 1) {
        const suit = Math.floor((Math.random() * 5) + 1);
        await page.click('div[name="suit"]>label:nth-of-type(' + suit + ')>lanfeust-bridge-suit>span:nth-of-type(1)>span');
        if (Math.random() < 0.25) {
            if (Math.random() < 0.2) {
                await page.click('label[name="redoubled"]');
            } else {
                await page.click('label[name="doubled"]');
            }
        }
        const declarer = Math.floor((Math.random() * 4) + 1);
        await page.click('div[name="declarer"]>label:nth-of-type(' + declarer + ')');
        const result = Math.floor((Math.random() * 5) - 2);
        if (result !== 0) {
            const el = page.locator('div[name="result"]>button:nth-of-type(' + (result < 0 ? 1 : 3) + ')');
            for (let i = 0; i < Math.abs(result); i++)
                await el.click();
        }
    }
    await page.click('button:text-is("Accept")');
}

test.describe('front-end App', () => {
    test('should run a mitchell tournament', async function ({ page }) {
        await page.goto('/');
        await page.click('text=Create new tournament');
        await expect(page).toHaveURL('/new-tournament');
        await page.fill('[placeholder="Tournament name"]', 'Test Mitchell');
        await page.fill('input[name="rounds"]', '3');
        await page.click('text=Players');
        for (let i = 0; i < 12; i++) {
            await page.fill('#player' + i, 'Player ' + (i + 1));
        }
        await page.click('text=Infos');
        await Promise.all([
          page.waitForNavigation(/*{ url: '/tournament/1' }*/),
          page.click('button:text-is("Create")')
        ]);
        await page.click('text=Start');

        // close alerts
        await expect(page.locator('[aria-label="Close"]')).toHaveCount(2);
        await page.click('[aria-label="Close"]');
        await expect(page.locator('[aria-label="Close"]')).toHaveCount(1);
        await page.click('[aria-label="Close"]');
        await expect(page.locator('[aria-label="Close"]')).toHaveCount(0);

        for (let round = 0; round < 3; round++) {
            await page.selectOption('select[name="currentPlayer"]', '0');
            await randomScore(page);
            await randomScore(page);

            await page.selectOption('select[name="currentPlayer"]', '4');
            await randomScore(page);
            await randomScore(page);

            await page.selectOption('select[name="currentPlayer"]', '8');
            await randomScore(page);
            await randomScore(page);

            if (round < 2) {
                await page.click('text=Next Round');
                await page.waitForSelector('text=Round ' + (round + 2));
            }
        }
        // finish tournament
        await page.click('text=Close');

        // close alert
        await page.click('[aria-label="Close"]');

        // navigate a bit in results
        await page.click('text=Player 2');
        await expect(page).toHaveURL(RegExp('/tournament/\\d/scoresheet/Player%202'));

        // click deal 5
        await page.click('a:text-is("5")');
        await expect(page).toHaveURL(RegExp('/tournament/\\d/deal/5'));
        await page.click('text=Next');
        await expect(page).toHaveURL(RegExp('/tournament/\\d/deal/6'));

        await page.click('text=Up');
        await expect(page).toHaveURL(RegExp('/tournament/\\d'));
        await page.click('text=Deals');
        await page.click('text=Deal 2');
        await expect(page).toHaveURL(RegExp('/tournament/\\d/deal/2'));
    });

    test('should run a 9-player individual', async function ({ page }) {
        await page.goto('/');
        await page.click('text=Create new tournament');
        await expect(page).toHaveURL('/new-tournament');
        await page.fill('[placeholder="Tournament name"]', 'Test Individual');
        await page.selectOption('select[name="movement"]', 'individual9');
        await page.selectOption('select[name="scoring"]', 'IMP');
        await page.fill('input[name="dealsPerRound"]', '3');
        await page.click('text=Players');
        for (let i = 0; i < 9; i++) {
            await page.fill('#player' + i, 'Player ' + (i + 1));
        }
        await page.click('text=Infos');
        await Promise.all([
          page.waitForNavigation(/*{ url: '/tournament/1' }*/),
          page.click('button:text-is("Create")')
        ]);
        await page.click('text=Start');

        // close alerts
        await expect(page.locator('[aria-label="Close"]')).toHaveCount(2);
        await page.click('[aria-label="Close"]');
        await expect(page.locator('[aria-label="Close"]')).toHaveCount(1);
        await page.click('[aria-label="Close"]');
        await expect(page.locator('[aria-label="Close"]')).toHaveCount(0);

        for (let round = 0; round < 9; round++) {
            const north1 = Math.floor(round / 3) * 3 + (round + 1) % 3;
            await page.selectOption('select[name="currentPlayer"]', north1.toString());
            await randomScore(page);
            await randomScore(page);
            await randomScore(page);

            const north2 = (north1 + 3) % 9;
            await page.selectOption('select[name="currentPlayer"]', north2.toString());
            await randomScore(page);
            await randomScore(page);
            await randomScore(page);

            if (round < 8) {
                await page.click('text=Next Round');
                await page.waitForSelector('text=Round ' + (round + 2));
            }
        }

        // finish tournament
        await page.click('text=Close');

        // close alert
        await page.click('[aria-label="Close"]');

        // navigate a bit in results
        await page.click('text="Player 1"');
        await expect(page).toHaveURL(RegExp('/tournament/\\d/scoresheet/Player%201'));

        // click deal 5
        await page.click('a:text-is("5")');
        await expect(page).toHaveURL(RegExp('/tournament/\\d/deal/5'));
        await page.click('text=Next');
        await expect(page).toHaveURL(RegExp('/tournament/\\d/deal/6'));

        await page.click('text=Up');
        await expect(page).toHaveURL(RegExp('/tournament/\\d'));
        await page.click('text=Deals');
        await page.click('text="Deal 2"');
        await expect(page).toHaveURL(RegExp('/tournament/\\d/deal/2'));
    });
});
