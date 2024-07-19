import { User } from './user.type'
import { SuccessResponse, SuccessResponseReq } from './utils.type'

export type AuthResponse = SuccessResponse<{
  access_token: string
  refresh_token: string
  user: User
}>

export type StudentResponse = SuccessResponseReq<{
  id: string
  fullName: string
  title: string
  subject: string
  price: 0
  class: string
  description: string
  methodLearning: string
  date: string
  timeStart: string
  timeEnd: string
}>
