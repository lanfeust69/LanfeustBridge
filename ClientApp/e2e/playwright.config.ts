import { PlaywrightTestConfig } from '@playwright/test';

const config: PlaywrightTestConfig = {
    timeout: 60000,
    globalSetup: require.resolve('./global-setup'),
    use: {
        // Tell all tests to load signed-in state from 'storageState.json'.
        storageState: 'storageState.json',
        baseURL: 'https://localhost:5001/',
        viewport: { width: 1280, height: 720 },
        ignoreHTTPSErrors: true,
        video: 'on-first-retry',
    },
    webServer: {
        command: 'cd ../.. && dotnet run',
        port: 5001,
        timeout: 120 * 1000,
        reuseExistingServer: !process.env.CI
    },
};

export default config;
