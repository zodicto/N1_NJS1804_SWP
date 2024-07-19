import { HttpStatusCode } from '../constant/HttpStatusCode.enum'
import {
  Request,
  ServiceTutor,
  ServiceTutorGet,
  ViewReviewRequestBody
} from '../types/request.type'

import { SuccessResponse, SuccessResponseReq } from '../types/utils.type'

import {
  AddQualification,
  CensorShipTutor,
  CreatServiceType,
  TutorProfile,
  UpdateTutorProfile
} from '../types/tutor.type'
import { JoinClassBody } from '../types/user.request.type'
import { User } from '../types/user.type'

import { getProfileFromLS } from '../utils/auth'
import http from '../utils/http'

const user = <User>getProfileFromLS()

export const tutorApi = {
  getProfileTT: async (id: string): Promise<TutorProfile> => {
    try {
      const response = await http.get<SuccessResponseReq<TutorProfile>>(
        `TutorProfile/getProfileTutor?id=${id}`
      )
      if (response.status === HttpStatusCode.Ok) {
        return response.data.data
      } else {
        throw new Error('Danh sách trống')
      }
    } catch (error) {
      // Handle network or API errors
      throw new Error('Thất bại trong việc lấy dữ liệu ')
    }
  },

  viewRequest: async (id: string): Promise<Request[]> => {
    try {
      const response = await http.get<SuccessResponse<Request[]>>(
        `Request/getAllApprovedRequest?id=${id}`
      )
      if (response.status === HttpStatusCode.Ok) {
        return response.data.data
      } else {
        throw new Error('Danh sách trống')
      }
    } catch (error) {
      // Handle network or API errors
      throw new Error('Thất bại trong việc lấy dữ liệu ')
    }
  },
  //  tham gia lớp
  joinClass: async (body: JoinClassBody) =>
    await http.post<SuccessResponse<any>>(
      `Request/join-request?requestId=${body.requestId}&id=${body.id}`
    ),

  createService: async (id: string, body: CreatServiceType) => {
    return await http.post<SuccessResponseReq<string>>(
      `Service/createService?id=${id}`,
      body
    )
  },

  updateService: async (idService: string, body: CreatServiceType) => {
    return await http.put<SuccessResponseReq<string>>(
      `Service/updateService?idService=${idService}`,
      body
    )
  },

  getReview: async (idTutor: string) => {
    try {
      const response = await http.get<
        SuccessResponseReq<ViewReviewRequestBody[]>
      >(`Review/GetReview?id=${idTutor}`)
      if (response.status === HttpStatusCode.Ok) {
        return response.data.data
      } else {
        throw new Error('Danh sách trống')
      }
    } catch (error) {
      // Handle network or API errors
      throw new Error('Thất bại trong việc lấy dữ liệu ')
    }
  },

  async updateProfileTT(id: string, body: UpdateTutorProfile) {
    // console.log('body của res khi call api',body)
    // console.log('người dùng user là',user)
    // console.log(' id người dùng user là',user.id)
    return await http.put<SuccessResponseReq<any>>(
      `TutorProfile/updateTutorProfile?id=${id}`,
      body
    )
  },

  addSubject: async (
    id: string,
    subjectName: string
  ): Promise<SuccessResponseReq<any>> => {
    try {
      const response = await http.post<SuccessResponseReq<any>>(
        `TutorProfile/addSubject?id=${id}&subjectName=${subjectName}`
      )

      return response.data // Return response data if needed
    } catch (error) {
      throw error // Re-throw error for handling in caller function
    }
  },
  addQualification: async (
    id: string,
    body: AddQualification
  ): Promise<SuccessResponseReq<any>> => {
    try {
      const response = await http.post<SuccessResponseReq<any>>(
        `TutorProfile/addQualification?id=${id}`,
        body
      )

      return response.data // Return response data if needed
    } catch (error) {
      throw error // Re-throw error for handling in caller function
    }
  },
  getSerivceByTutor: async (idTutor: string): Promise<ServiceTutorGet[]> => {
    try {
      const response = await http.get<SuccessResponseReq<ServiceTutorGet[]>>(
        `Service/getServices?id=${idTutor}`
      )
      return response.data.data
    } catch (error) {
      throw error
    }
  },
  deleteTutorService: async (
    idService: string
  ): Promise<SuccessResponseReq<ServiceTutor[]>> => {
    try {
      const response = await http.delete<SuccessResponseReq<any>>(
        `Service/deleteService?idService=${idService}`
      )
      return response.data // Return response data if needed
    } catch (error) {
      throw error // Re-throw error for handling in caller function
    }
  },

  //  lấy đơn
  getRegisterTutor: async (id: string) => {
    try {
      const response = await http.get<SuccessResponseReq<CensorShipTutor>>(
        `Tutor/getSignUpTutor?id=${id}`
      )
      return response.data
    } catch (error) {
      throw new Error('Thất bại trong việc lấy dữ liệu ')
    }
  }
}
