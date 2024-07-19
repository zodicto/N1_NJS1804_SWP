import {
  AcceptTutorBody,
  RequestBody,
  RequestTutorBody,
  SelecTutorReqBody,
  UpdateRequest
} from '../types/user.request.type'
import { SuccessResponse, SuccessResponseReq } from './../types/utils.type'

import { getProfileFromLS } from '../utils/auth'
import http from '../utils/http'

import { HttpStatusCode } from '../constant/HttpStatusCode.enum'
import {
  Classrequest,
  Request,
  ReviewServiceType,
  ReviewType,
  ServiceTutor
} from '../types/request.type'
import { TutorType } from '../types/tutor.type'
import { User } from '../types/user.type'
import { DataType } from '../pages/Sevice/components/ModalChooseService/ModalChooseService'

const user = <User>getProfileFromLS()

export const studentApi = {
  // Tạo yêu cầu
  createRequest: async (id: string, body: RequestBody) =>
    await http.post(`Request/createRequest?id=${id}`, body),

  // Lấy danh sách yêu cầu chờ duyệt
  async rejectRequest(id: string) {
    try {
      const response = await http.get<SuccessResponseReq<Request[]>>(
        `Request/getRejectRequest?id=${id}`
      )
      if (response.status === HttpStatusCode.Ok) {
        return response.data.data
      } else {
        throw new Error('Danh sách trống')
      }
    } catch (error) {
      console.error('Error fetching pending requests:', error)
      throw error
    }
  },

  // Lấy danh sách yêu cầu chờ duyệt
  async pendingRequest(id: string) {
    try {
      const response = await http.get<SuccessResponseReq<Request[]>>(
        `Request/getPedingRequest?id=${id}`
      )
      if (response.status === HttpStatusCode.Ok) {
        return response.data.data
      } else {
        throw new Error('Danh sách trống')
      }
    } catch (error) {
      console.error('Error fetching pending requests:', error)
      throw error
    }
  },

  // Lấy danh sách yêu cầu đã duyệt
  async approvedRequest(id: string) {
    try {
      const response = await http.get(`Request/getAppovedRequest?id=${id}`)
      if (response.status === HttpStatusCode.Ok) {
        return response.data.data
      } else {
        throw new Error('Danh sách trống')
      }
    } catch (error) {
      console.error('Lỗi trong quá trình xử lý:', error)
      throw error
    }
  },

  // Xem tất cả yêu cầu tham gia của gia sư
  viewAllTutorsJoinRequests: async (requestId: string) => {
    try {
      const response = await http.get<SuccessResponseReq<TutorType[]>>(
        `Request/getAllTutorsJoinRequest?idRequest=${requestId}`
      )
      if (response.status === HttpStatusCode.Ok) {
        return response.data.data
      } else {
        throw new Error('Danh sách trống')
      }
    } catch (error) {
      console.error('Lỗi trong quá trình xử lý:', error)
      throw error
    }
  },

  // // Chấp nhận yêu cầu từ một gia sư cụ thể
  acceptTutor: async (body: AcceptTutorBody) => {
    await http.post(
      `Request/selectTutor?idRequest=${body.idRequest}&idaccounttutor=${body.idTutor}`
    )
  },

  // // Đăng ký làm gia sư
  registerAsTutor: async (body: RequestTutorBody, id: string) =>
    await http.post<SuccessResponse<any>>(
      `Tutor/registerAsTutor?id=${id}`,
      body
    ),

  //  chỉnh sửa cập nhật gia sư
  reSignUpofTutor: async (body: RequestTutorBody, id: string) =>
    await http.put<SuccessResponse<any>>(
      `/Tutor/reSignUpOfTutor?id=${id}`,
      body
    ),

  updateRequest: async (body: UpdateRequest) =>
    await http.put<SuccessResponse<RequestBody>>(
      `Request/updateRequest?idRequest=${body.idReq}`,
      body.dataUpdate
    ),

  deleteRequest: async (id: string, idRequest: string) =>
    await http.delete<SuccessResponse<any>>(
      `Request/deleteRequest?id=${id}&idRequest=${idRequest}`
    ),

  // select Tutor
  selectTutor: async (body: SelecTutorReqBody) =>
    await http.post<SuccessResponse<any>>(
      `Request/selectTutor?idRequest=${body.idRequest}&idaccounttutor=${body.idTutor}`
    ),

  //  lấy lớp học đang diễn ra
  classActive(id: string) {
    return http.get<SuccessResponseReq<Classrequest>>(
      `Class/ViewClassRequest?id=${id}`
    )
  },

  createComplaint(body: {
    idUser: string
    description: string
    idAccountTutor: string
  }) {
    return http.post<SuccessResponseReq<string>>(
      `Complaint/CreateComplaint`,
      body
    )
  },

  classCompled(idClassRequest: string) {
    return http.put<SuccessResponseReq<any>>(
      `Class/completeClassRequest?idClassRequest=${idClassRequest}`
    )
  },

  getReview(id: string) {
    return http.get<SuccessResponseReq<any>>(`Review/GetReview?id=${id}`)
  },

  serviceCompled(idBooking: string) {
    return http.put<SuccessResponseReq<any>>(
      `Class/completeClassService?idBooking=${idBooking}`
    )
  },
  BookingServiceLearning: async (
    id: string,
    serviceID: string,
    body: DataType
  ) => {
    console.log('BookingServiceLearning', serviceID)

    try {
      const response = await http.post<SuccessResponseReq<string>>(
        `Service/bookingService?id=${id}&idService=${serviceID} `,
        body
      )
      if (response.status === HttpStatusCode.Ok) {
        return response
      } else {
        throw new Error('Danh sách trống')
      }
    } catch (error) {
      console.error('Lỗi trong quá trình xử lý:', error)
      throw error
    }
  },

  async GetAllService() {
    return await http.get<SuccessResponseReq<ServiceTutor[]>>(
      'Service/getAllService'
    )
  },

  CreateReview: async (body: ReviewType) => {
    return await http.post<SuccessResponseReq<string>>(
      'Review/CreateReviewRequest',
      body
    )
  },
  CreateServiceReview: async (body: ReviewServiceType) => {
    return await http.post<SuccessResponseReq<string>>(
      'Review/CreateReviewService',
      body
    )
  },

  //  xóa đơn
  deleteRegisterTutor: async (id: string) =>
    await http.delete<SuccessResponse<any>>(`Tutor/deleteSignUpTutor?id=${id} `)
}
