import { useParams } from 'react-router-dom'
import http from '../utils/http'
interface RegisterReqBody {
  username: string
  password: string
}
export interface ResATReqBody {
  qualificationName: string
  type: string
  field: string
  experience: Number
  specializedSkills: string
  imageQualification: string
}
const buildUrlWithParams = (url: string, params: ResATReqBody): string => {
  const queryParams = new URLSearchParams({
    qualificationName: params.qualificationName,
    type: params.type,
    field: params.field,
    experience: params.experience.toString(),
    specializedSkills: params.specializedSkills
  }).toString()

  return `${url}?${queryParams}`
}
const baseUrl = '/user/registerAsTutor'
export const regisApi = {
  register: (body: RegisterReqBody) => http.post<any>('/register', body), // nhận vào một đối tượng body có kiểu LoginReqBody, hàm sử dụng phương thức post của axios gửi yêu cầu đăng nhập đến endpoint

  registerAT: (body: ResATReqBody) => {
    const urlWithParams = buildUrlWithParams(baseUrl, body)
    return http.post<any>(urlWithParams, body.imageQualification)
  }
}
