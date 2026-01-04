import { test, expect } from '@playwright/test';
import { HomePage } from '../pages/HomePage';

test.describe('Sample E2E Tests', () => {
  test('should navigate to application and load page', async ({ page }) => {
    await page.goto('/');
    await expect(page).toHaveURL(/localhost:5173/);
    await page.waitForLoadState('domcontentloaded');
  });

  test('should demonstrate locator strategies', async ({ page }) => {
    await page.goto('/');

    const bodyElement = page.locator('body');
    await expect(bodyElement).toBeVisible();

    const headingByRole = page.getByRole('heading').first();
    if (await headingByRole.count() > 0) {
      await expect(headingByRole).toBeVisible();
    }

    const allLinks = page.locator('a');
    const linkCount = await allLinks.count();
    expect(linkCount).toBeGreaterThanOrEqual(0);
  });

  test('should demonstrate element assertions', async ({ page }) => {
    await page.goto('/');

    const body = page.locator('body');
    await expect(body).toBeVisible();
    await expect(body).not.toBeHidden();

    const htmlElement = page.locator('html');
    await expect(htmlElement).toBeAttached();
  });

  test('should demonstrate Page Object Model pattern', async ({ page }) => {
    const homePage = new HomePage(page);

    await homePage.goto();

    const isLoaded = await homePage.isPageLoaded();
    expect(isLoaded).toBe(true);

    await expect(page).toHaveURL(/localhost:5173/);
  });
});
