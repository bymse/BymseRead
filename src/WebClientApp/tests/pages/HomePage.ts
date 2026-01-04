import { type Page, type Locator } from '@playwright/test';

export class HomePage {
  readonly page: Page;
  readonly mainHeading: Locator;
  readonly navigationLinks: Locator;
  readonly buttons: Locator;

  constructor(page: Page) {
    this.page = page;
    this.mainHeading = page.locator('h1').first();
    this.navigationLinks = page.locator('nav a');
    this.buttons = page.locator('button');
  }

  async goto() {
    await this.page.goto('/');
  }

  async getHeadingText(): Promise<string> {
    return await this.mainHeading.textContent() ?? '';
  }

  async clickFirstButton() {
    await this.buttons.first().click();
  }

  async isPageLoaded(): Promise<boolean> {
    await this.page.waitForLoadState('domcontentloaded');
    return true;
  }
}
