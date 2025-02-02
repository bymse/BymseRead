import './global.css'
import { ErrorBoundary, LocationProvider, Router, Route, useLocation } from 'preact-iso'
import { BooksList } from './pages/BooksList/BooksList.tsx'
import { useEffect } from 'preact/hooks'
import { ToastProvider } from '@components/Toast/ToastContext.tsx'

export function App() {
  return (
    <LocationProvider>
      <ErrorBoundary>
        <ToastProvider>
          <Router>
            <Home path="/" />
            <Route path="/books" component={BooksList} />
          </Router>
        </ToastProvider>
      </ErrorBoundary>
    </LocationProvider>
  )
}

const Home = () => {
  const { route } = useLocation()

  useEffect(() => {
    route('/books')
  }, [])

  return <></>
}
