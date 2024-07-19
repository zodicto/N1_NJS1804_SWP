import { SuccessResponse } from './../types/utils.type'
import { AuthResponse } from '../types/auth.type'
import {
  LoginReqBody,
  LogoutReqBody,
  ResReqBody
} from '../types/user.request.type'

import http from '../utils/http'

export const authApi = {
  loginAccount: async (body: LoginReqBody) =>
    await http.post<AuthResponse>('/Account/login', body), // nhận vào một đối tượng body có kiểu LoginReqBody, hàm sử dụng phương thức post của axios gửi yêu cầu đăng nhập đến endpoint

  registerAccount: async (body: ResReqBody) =>
    await http.post<AuthResponse>('/Account/register', body),

  loginGG: async () => await http.post<AuthResponse>('Account/signin-google'),

  logoutAccount: async (body: LogoutReqBody) =>
    await http.post('Account/logout', body)
}
