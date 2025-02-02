import { ErrorBoundary, LocationProvider, Router, Route, useLocation } from 'preact-iso'
import { BooksList } from './pages/BooksList/BooksList.tsx'
import { useEffect } from 'preact/hooks'

export function App() {
  return (
    <LocationProvider>
      <ErrorBoundary>
        <Router>
          <Home path="/" />
          <Route path="/books" component={BooksList} />
        </Router>
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
