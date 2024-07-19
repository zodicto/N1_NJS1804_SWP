import { reqDeposit } from '../types/user.request.type'
import { User } from '../types/user.type'
import { SuccessResponseReq } from '../types/utils.type'
import http from '../utils/http'

export const paymentApi = {
  deposit: async (body: reqDeposit) => {
    return await http.post<SuccessResponseReq<string>>(`payment/payment`, body)
  },

  paymentcallback: async (body: User) => {
    const response = await http.get<any>(
      `payment/paymentCallBack?id=${body.id}`
    )
    return response.data
  }
}
