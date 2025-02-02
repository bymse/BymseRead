import styles from './NotFound.module.scss'
import { Button } from '@components/Button/Button.tsx'
import { Header } from '@components/Header/Header.tsx'
import illustration from '@assets/not-found-illustration.svg'

export const NotFound = () => {
  return (
    <>
      <Header />
      <div className={styles.wrapper}>
        <h2 className={styles.text}>
          We couldn’t find the page
          <br />
          you were looking for
        </h2>
        <div>
          <Button title="Go home" />
        </div>
        <div className={styles.illustration}>
          <img src={illustration} alt="Not found" width="60" height="60" />
        </div>
      </div>
    </>
  )
}
