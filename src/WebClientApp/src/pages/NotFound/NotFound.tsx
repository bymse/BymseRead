import styles from './NotFound.module.scss'
import { Button } from '@components/Button/Button.tsx'
import { Header } from '@components/Header/Header.tsx'
import illustration from '@assets/not-found-illustration.svg?inline'
import { usePageTitle } from '@hooks/usePageTitle.ts'

export const NotFound = () => {
  usePageTitle()

  return (
    <>
      <Header />
      <div className={styles.wrapper} data-testid="not-found-page">
        <h2 className={styles.text}>
          We couldn’t find the page
          <br />
          you were looking for
        </h2>
        <div>
          <Button title="Go home" href="/" />
        </div>
        <div className={styles.illustration}>
          <img src={illustration} alt="Not found" width="60" height="60" />
        </div>
      </div>
    </>
  )
}
