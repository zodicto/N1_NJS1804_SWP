import React, { createContext, useEffect, useState } from 'react'
import { User } from '../types/user.type'
import {
  getAccessTokenFromLS,
  getProfileFromLS,
  getRefreshTokenFromLS
} from '../utils/auth'

interface AppContextInterface {
  isAuthenticated: boolean
  setIsAuthenticated: React.Dispatch<React.SetStateAction<boolean>>
  refreshToken: string
  setRefreshToken: React.Dispatch<React.SetStateAction<string>>
  profile: User | null
  setProfile: React.Dispatch<React.SetStateAction<User | null>>
  reset: () => void
}

const initialAppContext: AppContextInterface = {
  isAuthenticated: Boolean(getAccessTokenFromLS()),
  setIsAuthenticated: () => null,
  profile: getProfileFromLS(),
  setProfile: () => null,
  refreshToken: getRefreshTokenFromLS() || '',
  setRefreshToken: () => null,
  reset: () => null
}

export const AppContext = createContext<AppContextInterface>(initialAppContext)

export const AppProvider = ({ children }: { children: React.ReactNode }) => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(
    initialAppContext.isAuthenticated
  )
  const [refreshToken, setRefreshToken] = useState<string>(
    initialAppContext.refreshToken
  )
  const [profile, setProfile] = useState<User | null>(initialAppContext.profile)

  const reset = () => {
    setIsAuthenticated(false)
    setProfile(null)
  }

  return (
    <AppContext.Provider
      value={{
        isAuthenticated,
        setIsAuthenticated,
        profile,
        setProfile,
        refreshToken,
        setRefreshToken,
        reset
      }}
    >
      {children}
    </AppContext.Provider>
  )
}
