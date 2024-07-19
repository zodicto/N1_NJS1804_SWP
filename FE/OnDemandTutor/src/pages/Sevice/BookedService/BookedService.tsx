import { useContext, useState } from 'react'
import { TutorRep, User, UserRep } from '../../../types/user.type'
import { keepPreviousData, useMutation, useQuery } from '@tanstack/react-query'
import { studentApi } from '../../../api/student.api'
import userApi from '../../../api/user.api'
import { toast } from 'react-toastify'
import DetailInfor from '../../MyClass/Detail' // sửa lại để xài chung
import { BookedServices } from '../../../types/request.type'
import ReviewService from '../ReviewService'
import { AppContext } from '../../../context/app.context'
import { Modal } from 'antd'

import { statusClass } from '../../../constant/status.class'
import Pagination from '../../../components/Pagination'
import { roles } from '../../../constant/roles'

export default function BookedService() {
  const { profile } = useContext(AppContext)

  const { data, refetch } = useQuery({
    queryKey: ['Account'],
    queryFn: () => userApi.ViewClassService(profile?.id as string),
    enabled: !!profile?.id,
    placeholderData: keepPreviousData
  })

  const serviceMutation = useMutation({
    mutationFn: (idBook: string) => studentApi.serviceCompled(idBook)
  })

  const [selectedService, setSelectedService] = useState<string | null>(null)
  const [hovered, setHovered] = useState<string | null>(null)
  const [isModalOpen, setIsModalOpen] = useState(false)
  const [showReviewModal, setShowReviewModal] = useState(false)
  const [currentPage, setCurrentPage] = useState(1)
  const [completedServiceId, setCompletedServiceId] = useState<string | null>(
    null
  )
  const itemsPerPage = 4 // Số phần tử trên mỗi trang

  const handleCompleteClass = (id: string) => {
    serviceMutation.mutate(id, {
      onSuccess: () => {
        toast.success('Kết thúc lớp thành công')
        setSelectedService(id)
        setShowReviewModal(true)
        setCompletedServiceId(id) // Mark service as completed
        refetch()
      },
      onError: (data) => {
        toast.error(data.message)
      }
    })
  }

  const handleOpenModal = (idBooking: string) => {
    setSelectedService(idBooking)
    setIsModalOpen(true)
  }

  const handleCloseModal = () => {
    setIsModalOpen(false)
    setShowReviewModal(false) // Close review modal
  }

  const serviceList = data || []

  // Tính toán số lượng phần tử trên từng trang
  const indexOfLastItem = currentPage * itemsPerPage
  const indexOfFirstItem = indexOfLastItem - itemsPerPage
  const currentItems = serviceList.slice(indexOfFirstItem, indexOfLastItem)

  // Xử lý thay đổi trang
  const handlePageChange = (page: number) => {
    setCurrentPage(page)
  }

  return (
    <div className='container'>
      {profile?.roles.toLowerCase() === roles.tutor ? (
        <>
          <div className='text-center text-wrap border-b-2 border bg-slate-50 '>
            Danh sách dịch vụ của bạn đã được đăng kí bởi học sinh
          </div>
        </>
      ) : (
        <div className='text-center text-lg text-wrap bg-slate-50 '>
          Dịch vụ bạn đã đăng kí
        </div>
      )}

      {serviceList.length === 0 ? (
        <div className='text-center text-red-500 mt-10 text-lg'>
          Bạn không có lớp học nào
        </div>
      ) : (
        <>
          {currentItems.map((service: BookedServices) => (
            <div
              key={service.idBooking}
              className='rounded-3xl my-5 grid-cols-2'
              onMouseEnter={() => setHovered(service.idBooking)}
              onMouseLeave={() => setHovered(null)}
            >
              {/* Parent */}
              <div className='w-[33rem] py-auto h-auto rounded-3xl px-5 hover:shadow-2xl hover:shadow-black   mx-auto transition-shadow duration-500'>
                <div className='my-2'>
                  <h2 className='text-red-600 text-2xl'>{service.title}</h2>
                </div>
                <div className='text-[1rem] text-left'>
                  <div>
                    Môn dạy:{' '}
                    <span className='text-blue-500 font-bold text-md'>
                      {service.subject}
                    </span>
                  </div>
                  <div className='my-1'>
                    Lớp dạy:{' '}
                    <span className='text-blue-500 font-bold text-md'>
                      {service.class}
                    </span>
                  </div>
                  <div className='my-1'>
                    Giá toàn dịch vụ:{' '}
                    <span className='text-red-400 font-bold text-md'>
                      {service.price}
                    </span>{' '}
                    VNĐ
                  </div>
                  <div className='my-1'>
                    Ngày học:{' '}
                    <span className='text-black font-bold text-md'>
                      {service.date}
                    </span>
                  </div>
                  <div className='my-1'>
                    Thời gian bắt đầu:{' '}
                    <span className='text-black font-bold text-md'>
                      {service.timeSlot}
                    </span>
                  </div>

                  <div className='my-1'>
                    Hình thức:{' '}
                    <span className='text-black font-bold text-md'>
                      {service.learningMethod}
                    </span>
                  </div>
                  <div className='my-1'>
                    Mô tả:{' '}
                    <span className='text-black font-bold text-md'>
                      {service.description}
                    </span>
                  </div>
                  <div className='my-1'>
                    Trạng thái:{' '}
                    <span className='text-black font-bold text-md'>
                      {service.status}
                    </span>
                  </div>
                </div>
                <div
                  className={`flex mt-2 justify-between transition-max-height duration-300 ease-in-out mx-auto ${
                    hovered === service.idBooking
                      ? 'max-h-20'
                      : 'max-h-0 overflow-hidden'
                  }`}
                >
                  <div className='w-full flex items-center justify-center'>
                    {service.status.toLowerCase() === statusClass.complete ? (
                      <button
                        onClick={() => handleOpenModal(service.idBooking)}
                        className='w-full bg-pink-400 text-white font-bold py-2 px-4 rounded-md hover:bg-pink-200'
                      >
                        Chi tiết
                      </button>
                    ) : (
                      <button
                        onClick={() => handleCompleteClass(service.idBooking)}
                        className='w-[49%] bg-black text-white font-bold py-2 px-4 rounded-md hover:bg-gray-400'
                      >
                        Kết thúc lớp
                      </button>
                    )}
                  </div>
                  <hr />
                </div>
              </div>
              {isModalOpen && selectedService === service.idBooking && (
                <Modal
                  title='Chi tiết lớp học'
                  visible={isModalOpen}
                  onCancel={handleCloseModal}
                  className='min-w-[90rem]'
                  footer={null}
                >
                  <DetailInfor
                    User={service.user as UserRep}
                    Tutor={service.tutor as TutorRep}
                  />
                </Modal>
              )}
              {showReviewModal && selectedService === service.idBooking && (
                <ReviewService idBooking={service.idBooking} />
              )}
              <hr />
            </div>
          ))}
          <Pagination
            totalItems={serviceList.length}
            itemsPerPage={itemsPerPage}
            currentPage={currentPage}
            onPageChange={handlePageChange}
          />
        </>
      )}
    </div>
  )
}
