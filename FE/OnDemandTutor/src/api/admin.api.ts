import { HttpStatusCode } from 'axios'
import { AdminTutorProfile, AdminTutorType } from '../types/tutor.type'
import { SuccessResponseReq } from '../types/utils.type'
import http from '../utils/http'
import { RequestModerator } from '../types/request.type'
import { ChartData } from '../types/chart.type'
import { complaintType } from '../types/complaint.type'

export const adminAPI = {
  async getTutorList() {
    try {
      const response = await http.get<SuccessResponseReq<AdminTutorProfile[]>>(
        `Tutor/viewAllTutor`
      )
      if (response.status === HttpStatusCode.Ok) {
        return response.data.data
      } else {
        throw new Error('responece trống')
      }
    } catch (error) {
      throw new Error('Lỗi khi sử dụng api')
    }
  },
  async getRequestTutorReg() {
    try {
      const response = await http.get<SuccessResponseReq<AdminTutorType[]>>(
        `/modaretor/viewListTutor`
      )
      if (response.status === HttpStatusCode.Ok) {
        return response.data.data
      } else {
        throw new Error('Danh sách trống')
      }
    } catch (error) {
      throw new Error('Failed to fetch data')
    }
  },
  async deleteAccount(id: string) {
    try {
      const response = await http.delete<SuccessResponseReq<any>>(
        `Account/DeleteAccount?id=${id}`
      )
      if (response.status === HttpStatusCode.Ok) {
        return response.data.data
      } else {
        throw new Error('Danh sách  trống')
      }
    } catch (error) {
      throw new Error('Lỗi khi sử dụng api')
    }
  },
  async getStudentReq() {
    try {
      const response = await http.get<SuccessResponseReq<RequestModerator[]>>(
        `/modaretor/viewRequest`
      )
      if (response.status === HttpStatusCode.Ok) {
        return response.data.data
      } else {
        throw new Error('Danh sách trống')
      }
    } catch (error) {
      throw new Error('Failed to fetch data')
    }
  },
  async getStudentReqApproved() {
    try {
      const response = await http.get<SuccessResponseReq<RequestModerator[]>>(
        `/Admin/viewAllRequestApproved`
      )
      if (response.status === HttpStatusCode.Ok) {
        return response.data.data
      } else {
        throw new Error('Danh sách trống')
      }
    } catch (error) {
      throw new Error('Failed to fetch data')
    }
  },
  async getStudentReqRejected() {
    try {
      const response = await http.get<SuccessResponseReq<RequestModerator[]>>(
        `/Admin/viewAllRequestReject`
      )
      if (response.status === HttpStatusCode.Ok) {
        return response.data.data
      } else {
        throw new Error('Danh sách trống')
      }
    } catch (error) {
      throw new Error('Failed to fetch data')
    }
  },
  async getStudentList() {
    try {
      const response = await http.get<SuccessResponseReq<DataTypeStu[]>>(
        `Student/GetAllStudent`
      )
      if (response.status === HttpStatusCode.Ok) {
        return response.data.data
      } else {
        throw new Error('Danh sách trống')
      }
    } catch (error) {
      throw new Error('Failed to fetch data')
    }
  },
  async deleteReq(id: string) {
    try {
      const response = await http.delete<SuccessResponseReq<any>>(``)
      if (response.status === HttpStatusCode.Ok) {
        return response.data.data
      } else {
        throw new Error('responece trống')
      }
    } catch (error) {
      throw new Error('Lỗi khi sử dụng api')
    }
  },
  async getAmountStudents() {
    try {
      const response = await http.get<SuccessResponseReq<number>>(
        `/Student/GetAmountStudent`
      )
      if (response.status === HttpStatusCode.Ok) {
        return response.data.data
      } else {
        throw new Error('Failed to fetch data')
      }
    } catch (error) {
      throw new Error('Failed to fetch data')
    }
  },
  async getAmountTutors() {
    try {
      const response = await http.get<SuccessResponseReq<number>>(
        `/Tutor/ViewAmountTutor`
      )
      if (response.status === HttpStatusCode.Ok) {
        return response.data.data
      } else {
        throw new Error('Failed to fetch data')
      }
    } catch (error) {
      throw new Error('Failed to fetch data')
    }
  },
  // lấy theo tháng này(this month)
  async getMonthlyRevenue() {
    try {
      const response = await http.get<SuccessResponseReq<number>>(
        `/Revenue/ViewRevenueThisMonth`
      )
      if (response.status === HttpStatusCode.Ok) {
        return response.data.data
      } else {
        throw new Error('Failed to fetch data')
      }
    } catch (error) {
      throw new Error('Failed to fetch data')
    }
  },
  //  theo năm
  async getRevenueByYear(year: number) {
    try {
      const response = await http.get<SuccessResponseReq<ChartData[]>>(
        `/Revenue/ViewRevenueByYear?year=${year}`
      )
      if (response.status === HttpStatusCode.Ok) {
        console.log(' response.data.data', response.data.data)
        return response.data.data
      } else {
        throw new Error('Failed to fetch data')
      }
    } catch (error) {
      throw new Error('Failed to fetch data')
    }
  },
  async getTransaction() {
    try {
      const response = await http.get<SuccessResponseReq<any>>(
        `Transaction/ViewAllTransaction`
      )
      if (response.status === HttpStatusCode.Ok) {
        return response.data.data
      } else {
        throw new Error('Failed to fetch data')
      }
    } catch (error) {
      throw new Error('Failed to fetch data')
    }
  },

  //  viewAllReivew của admin
  async getComplaintList() {
    try {
      const response = await http.get<SuccessResponseReq<complaintType[]>>(
        'Complaint/ViewAllComplaint'
      )
      if (response.status === HttpStatusCode.Ok) {
        console.log(' response.data.data', response.data.data)
        return response.data
      } else {
        throw new Error('Failed to fetch data')
      }
    } catch (error) {
      throw new Error('Failed to fetch data')
    }
  }

  //  //  viewAllReivew
  // async getComplaintList() {
  //   try {
  //     const response = await http.get<SuccessResponseReq<complaintType[]>>(
  //       'Complaint/ViewAllComplaint'
  //     )
  //     if (response.status === HttpStatusCode.Ok) {
  //       console.log(' response.data.data', response.data.data)
  //       return response.data
  //     } else {
  //       throw new Error('Failed to fetch data')
  //     }
  //   } catch (error) {
  //     throw new Error('Failed to fetch data')
  //   }
  // }
}
