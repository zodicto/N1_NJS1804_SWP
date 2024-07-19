import axios, { AxiosError, AxiosInstance, HttpStatusCode } from 'axios'
import {
  clearLS,
  getAccessTokenFromLS,
  getRefreshTokenFromLS,
  setAccessTokenToLS,
  setProfileToLS,
  setRefreshTokenToLS
} from './auth'

import config from '../constant/config'
import { AuthResponse } from '../types/auth.type'

import { toast } from 'react-toastify'
import { pathAuth } from '../constant/path'

class Http {
  instance: AxiosInstance
  private accessToken: string
  private refreshToken: string

  constructor() {
    this.accessToken = getAccessTokenFromLS()
    this.refreshToken = getRefreshTokenFromLS()
    this.instance = axios.create({
      baseURL: config.baseUrl,
      timeout: 100000,
      headers: {
        'Content-Type': 'application/json'
      }
    })

    this.instance.interceptors.request.use(
      (config) => {
        console.log(config)

        if (this.accessToken && config.headers) {
          config.headers.Authorization = `Bearer ${this.accessToken}`
        }
        return config
      },
      (error) => {
        return Promise.reject(error)
      }
    )

    this.instance.interceptors.response.use(
      (response) => {
        const { url } = response.config
        console.log(url)

        if (url === pathAuth.login || url === pathAuth.register) {
          const data = response.data as AuthResponse
          console.log(data)
          this.accessToken = `Bearer ${data.data.access_token}`
          console.log('accessToken', data.data.access_token)

          this.refreshToken = data.data.refresh_token
          console.log('refreshToken', data.data.refresh_token)

          setAccessTokenToLS(this.accessToken)

          console.log('User', data.data.user)

          setRefreshTokenToLS(this.refreshToken)
          setProfileToLS(data.data.user)
        } else if (url === pathAuth.logout) {
          clearLS()
        }
        return response
      },
      (error: AxiosError) => {
        if (error.response?.status !== HttpStatusCode.UnprocessableEntity) {
          const data: any | undefined = error.response?.data
          console.log(data)

          const message = data?.message || error.message

          console.log(message)

          toast.error(message)
        }
        if (error.response?.status === HttpStatusCode.Unauthorized) {
          // 401
          clearLS()
        }
        return Promise.reject(error)
      }
    )
  }
}

const http = new Http().instance

export default http
