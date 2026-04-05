import { expect, test } from '@playwright/test'
import { registerUser } from '../actions/authActions.js'
import { addBookThenOpenReader } from '../actions/booksActions.js'
import { SITE_URL } from '../utils/constants.js'
import { openBookmarksPanel } from '../actions/bookmarksActions.js'
import {
  clickFirstOutlineChevron,
  clickFirstOutlineLink,
  closeContentsPanel,
  getActiveOutlineId,
  openContentsPanel,
} from '../actions/contentsActions.js'
import { getPageFromHeader, setPageInHeader } from '../actions/readerActions.js'

test.describe('Contents panel', () => {
  test('ContentsAndBookmarks_Should_BeMutuallyExclusive', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerUser(page)
    await addBookThenOpenReader(page, 'with_contents.pdf')

    await openContentsPanel(page)
    await expect(page.getByTestId('bookmarks-panel')).toBeHidden()

    await openBookmarksPanel(page)
    await expect(page.getByTestId('contents-panel')).toBeHidden()

    await openContentsPanel(page)
    await expect(page.getByTestId('bookmarks-panel')).toBeHidden()
  })

  test('ClickingInternalOutlineRow_Should_NavigateAndKeepPanelOpenOnDesktop', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerUser(page)
    await addBookThenOpenReader(page, 'with_contents.pdf')

    await setPageInHeader(page, 120)
    const pageBeforeNavigation = await getPageFromHeader(page)

    await openContentsPanel(page)
    await clickFirstOutlineLink(page)

    await expect
      .poll(async () => {
        return getPageFromHeader(page)
      })
      .not.toBe(pageBeforeNavigation)

    await expect(page.getByTestId('contents-panel')).toBeVisible()
  })

  test('ActiveRowAndExpansion_Should_UpdateAndResetOnReopen', async ({ page }) => {
    await page.goto(SITE_URL)
    await registerUser(page)
    await addBookThenOpenReader(page, 'with_contents.pdf')

    await openContentsPanel(page)
    const activeIdBefore = await getActiveOutlineId(page)

    await setPageInHeader(page, 220)
    await expect
      .poll(async () => {
        return getActiveOutlineId(page)
      })
      .not.toBe(activeIdBefore)

    const rowCountBeforeExpand = await page.getByTestId('contents-outline-row').count()
    await clickFirstOutlineChevron(page)
    await expect
      .poll(async () => {
        return page.getByTestId('contents-outline-row').count()
      })
      .toBeGreaterThan(rowCountBeforeExpand)

    const expandedRowCount = await page.getByTestId('contents-outline-row').count()
    await closeContentsPanel(page)
    await openContentsPanel(page)

    await expect
      .poll(async () => {
        return page.getByTestId('contents-outline-row').count()
      })
      .toBeLessThan(expandedRowCount)
  })
})
