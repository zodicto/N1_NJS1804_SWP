import {
  ChangePasswordReqBody,
  ForgotPasswordReqBody,
  UpdateProfileBody,
  ViewClassRequestBody
} from './../types/user.request.type'
import { Notificate, User } from '../types/user.type'
import { SuccessResponse, SuccessResponseReq } from '../types/utils.type'
import { getProfileFromLS } from '../utils/auth'
import http from '../utils/http'
import { HttpStatusCode } from 'axios'

const user = getProfileFromLS()

//  id: string
//  roles: string
//  gender: string
//  email: string
//  fullName: string
//  date_of_birth: ''
//  address: string
//  phone: string
//  avatar: string
//  accountBalance: string

const userApi = {
  getProfile(id: string) {
    return http.get<SuccessResponseReq<User>>(`Account/getProfile?id=${id}`)
  },

  // cập nhật
  async updateProfile(body: UpdateProfileBody) {
    // console.log('body của res khi call api',body)
    // console.log('người dùng user là',user)
    // console.log(' id người dùng user là',user.id)
    return await http.put<SuccessResponseReq<User>>(
      `Account/updateProfile?id=${user.id}`,
      body
    )
  },

  async changePassword(body: ChangePasswordReqBody) {
    // Gửi yêu cầu HTTP PUT
    return await http.put(`Account/ChangePassword?id=${user.id}`, body)
  },

  async forgotPassword(body: ForgotPasswordReqBody) {
    return await http.put(`Account/ForgotPassword?Email=${body.email}`)
  },

  ViewClassService: async (id: string) => {
    try {
      const response = await http.get<SuccessResponseReq<any>>(
        `/Class/ViewClassService?id=${id}`
      )
      console.log('response', response)
      if (response.status === HttpStatusCode.Ok) {
        return response.data.data
      } else {
        throw new Error('Danh sách trống')
      }
    } catch (error) {
      throw new Error('Failed to fetch data')
    }
  },

  getNotification: async (id: string) => {
    try {
      const response = await http.get<SuccessResponseReq<Notificate[]>>(
        `Notification/ViewAllNotification?id=${id}`
      )
      console.log('response', response)
      if (response.status === HttpStatusCode.Ok) {
        return response.data.data
      } else {
        throw new Error('Danh sách trống')
      }
    } catch (error) {
      throw new Error('Failed to fetch data')
    }
  },

  updateNotification: async (id: string) => {
    return await http.put<SuccessResponse<any>>(
      `Notification/UpdateStatusNotification?id=${id}`
    )
  }
}

export default userApi
