import './global.css'
import { ErrorBoundary, LocationProvider, Router, Route, useLocation } from 'preact-iso'
import { BooksList } from './pages/BooksList/BooksList.tsx'
import { useEffect } from 'preact/hooks'
import { ToastProvider } from '@components/Toast/ToastContext.tsx'
import { NotFound } from './pages/NotFound/NotFound.tsx'
import { Book } from './pages/Book/Book.tsx'
import { useRegisterSW } from 'virtual:pwa-register/preact'

export function App() {
  return (
    <LocationProvider scope={/^((?!(web-api)).)*$/}>
      <ErrorBoundary>
        <ToastProvider>
          <Routes />
        </ToastProvider>
      </ErrorBoundary>
    </LocationProvider>
  )
}

const Routes = () => {
  useRegisterSW()

  return (
    <Router>
      <Home path="/" />
      <Route path="/books" component={BooksList} />
      <Route path="/books/:id" component={Book} />
      <NotFound default />
    </Router>
  )
}

const Home = () => {
  const { route } = useLocation()

  useEffect(() => {
    route('/books')
  }, [])

  return <></>
}
