import React, { useContext, useEffect } from 'react'
import { ToastContainer } from 'react-toastify'
import './App.css'
import useRouteElements from './useRouteElement'
import 'react-toastify/dist/ReactToastify.css'
import { LocalStrorageEventTarget } from './utils/utils'
import { AppContext } from './context/app.context'

function App() {
  const routeElements = useRouteElements()
  const { reset } = useContext(AppContext)

  useEffect(() => {
    // Listen for 'clearLS' events and call the reset function
    LocalStrorageEventTarget.addEventListener('clearLS', reset)

    // Clean up the event listener when the component unmounts
    return () => {
      LocalStrorageEventTarget.removeEventListener('clearLS', reset)
    }
  }, [reset])

  return (
    <div>
      {routeElements}
      <ToastContainer />
    </div>
  )
}

export default App
