import { User } from '../types/user.type'

//  phương thức nèa
export const getAccessTokenFromLS = () =>
  localStorage.getItem('access_token') || ''

export const getRefreshTokenFromLS = () =>
  localStorage.getItem('refresh_token') || ''

// Implementation of setAccessTokenToLS
export const setAccessTokenToLS = (access_token: string) =>
  localStorage.setItem('access_token', access_token)

export const setRefreshTokenToLS = (refresh_token: string) => {
  localStorage.setItem('refresh_token', refresh_token)
}

export const clearLS = () => {
  localStorage.removeItem('access_token')
  localStorage.removeItem('refresh_token')
  localStorage.removeItem('profile')
}

export const getProfileFromLS = () => {
  //  lấy xuống là string
  const result = localStorage.getItem('profile')
  // parse thành obj để xài
  return result ? JSON.parse(result) : null
}

// lưu ở localStroge
export const setProfileToLS = (profile: User) => {
  localStorage.setItem('profile', JSON.stringify(profile))
}
