import { expect, Page } from '@playwright/test'

export async function openContentsPanel(page: Page): Promise<void> {
  const panel = page.getByTestId('contents-panel')
  if (!(await panel.isVisible())) {
    const contentsButton = page.getByTestId('reader-header-contents-button')
    await contentsButton.waitFor({ state: 'visible' })
    await contentsButton.click()
  }
  await panel.waitFor({ state: 'visible' })
}

export async function closeContentsPanel(page: Page): Promise<void> {
  const panel = page.getByTestId('contents-panel')
  if (await panel.isVisible()) {
    await page.getByTestId('contents-close-button').click()
  }
  await expect(panel).toBeHidden()
}

export async function clickFirstOutlineChevron(page: Page): Promise<void> {
  await page.getByTestId('contents-outline-chevron').first().click()
}

export async function clickFirstOutlineLink(page: Page): Promise<void> {
  await page.getByTestId('contents-outline-link').first().click()
}

export async function getActiveOutlineId(page: Page): Promise<string | null> {
  const activeRow = page.locator('[data-testid="contents-outline-row"][data-active="true"]').first()
  await expect(activeRow).toBeVisible()
  return activeRow.getAttribute('data-outline-id')
}
