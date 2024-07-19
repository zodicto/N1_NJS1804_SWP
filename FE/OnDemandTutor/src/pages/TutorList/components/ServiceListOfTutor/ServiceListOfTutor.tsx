import { useQuery } from '@tanstack/react-query'
import { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'

import { ServiceTutorGet } from '../../../../types/request.type'
import ScheduleFormToChoose from '../../../Sevice/components/ScheduleFormToChose'
import ModalChooseService from '../../../Sevice/components/ModalChooseService'
import Pagination from '../../../../components/Pagination'
import { tutorApi } from '../../../../api/tutor.api'

export default function ServiceListOfTutor() {
  const { tutorId: idTutorParams } = useParams()
  const [selectedClassIndex, setSelectedClassIndex] = useState<number | null>(
    null
  )
  const [selectedDate, setSelectedDate] = useState<string>('')
  const [selectedTimeSlot, setSelectedTimeSlot] = useState<string | null>('')
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false)
  const [classData, setClassData] = useState<ServiceTutorGet[]>([])
  const [selectedClassInfo, setSelectedClassInfo] =
    useState<ServiceTutorGet | null>(null)

  const { data: Services, refetch } = useQuery({
    queryKey: ['allServiceOfTutor'],
    queryFn: () => tutorApi.getSerivceByTutor(idTutorParams as string)
  })

  useEffect(() => {
    if (Services) {
      setClassData(Services)
    }
  }, [Services])

  const handleDateChange = (classIndex: number, date: string) => {
    setSelectedClassIndex(classIndex)
    setSelectedDate(date)
    setSelectedTimeSlot(null)
  }

  const handleTimeSlotChange = (timeSlot: string) => {
    setSelectedTimeSlot(timeSlot)
  }

  const getDayOfWeek = (dateString: string) => {
    const date = new Date(dateString)
    return date.toLocaleDateString('vi-VN', { weekday: 'long' })
  }

  const openModal = (classInfo: ServiceTutorGet) => {
    setSelectedClassInfo(classInfo)
    setIsModalOpen(true)
  }

  const closeModal = () => {
    setIsModalOpen(false)
  }

  const confirmSelection = () => {
    // Handle the confirmation logic here
    closeModal()
  }

  // phần hiện xem thêm cho decription
  const [isDescriptionExpanded, setIsDescriptionExpanded] = useState(false)

  const toggleDescription = () => {
    setIsDescriptionExpanded(!isDescriptionExpanded)
  }

  const renderDescription = (description: string) => {
    const maxLength = 100
    if (description.length <= maxLength) {
      return description
    }
    return isDescriptionExpanded
      ? description
      : `${description.slice(0, maxLength)}...`
  }

  const [currentPage, setCurrentPage] = useState(1)
  const itemsPerPage = 5

  // Calculate paginated data
  const startIndex = (currentPage - 1) * itemsPerPage
  const paginatedData = classData.slice(startIndex, startIndex + itemsPerPage)

  const handlePageChange = (page: number) => {
    setCurrentPage(page)
  }

  return (
    <div className='w-2/3 border mx-auto grid gap-4'>
      <div className='text-wrap border-b-2 border bg-slate-50 '>
        Danh sách Lớp học của gia sư
      </div>
      <hr />
      {paginatedData.length > 0 ? (
        paginatedData.map((item, classIndex) => (
          <div
            key={startIndex + classIndex}
            className='w-full bg-transparent border-2 rounded-2xl grid grid-cols-2 hover:shadow-xl transition-shadow translate-x-4 duration-700 shadow-md gap-2 bg-white'
          >
            <div className='col-span-1 p-4'>
              <h2 className='text-xl font-bold mb-2'>
                {item.serviceDetails.title}
              </h2>
              <div className='pl-10 p-7 bg-gray-100 rounded-lg shadow-md'>
                <div className='text-left h-full mx-auto'>
                  <p className='mb-2'>
                    <strong className='text-pink-500'>Tiêu đề:</strong>{' '}
                    <span
                      className={
                        item.serviceDetails.title
                          ? 'text-gray-800'
                          : 'text-red-500'
                      }
                    >
                      {item.serviceDetails.title
                        ? item.serviceDetails.title
                        : 'Title trống'}
                    </span>
                  </p>
                  <p className='mb-2'>
                    <strong className='text-blue-700'>Môn học:</strong>{' '}
                    <span className='text-gray-800'>
                      {item.serviceDetails.subject}
                    </span>
                  </p>
                  <p className='mb-2'>
                    <strong className='text-blue-700'>Lớp:</strong>{' '}
                    <span className='text-gray-800'>
                      {item.serviceDetails.class}
                    </span>
                  </p>
                  <p className='mb-2'>
                    <strong className='text-blue-700'>Phương thức học:</strong>{' '}
                    <span className='text-gray-800'>
                      {item.serviceDetails.learningMethod}
                    </span>
                  </p>
                  <p className='mb-2'>
                    <strong className='text-blue-700'>Giá trên một giờ:</strong>{' '}
                    <span className='text-green-700'>
                      {item.serviceDetails.pricePerHour} VNĐ
                    </span>
                  </p>
                  <p>
                    <strong className='text-blue-700'>Mô tả:</strong>{' '}
                    <span className='text-gray-800'>
                      {renderDescription(item.serviceDetails.description)}
                    </span>
                    {item.serviceDetails.description.length > 100 && (
                      <button
                        onClick={toggleDescription}
                        className='text-blue-500 ml-2 hover:underline'
                      >
                        {isDescriptionExpanded ? 'Ẩn bớt' : 'Xem thêm'}
                      </button>
                    )}
                  </p>
                </div>
              </div>
            </div>
            <div className='col-span-1 p-4'>
              <p>
                <strong>Thời gian:</strong>
              </p>
              <ScheduleFormToChoose
                schedule={item.serviceDetails.schedule}
                classIndex={classIndex}
                selectedDate={
                  selectedClassIndex === classIndex ? selectedDate : ''
                }
                handleDateChange={handleDateChange}
                handleTimeSlotChange={handleTimeSlotChange}
                getDayOfWeek={getDayOfWeek}
              />
              <button
                onClick={() => openModal(item)}
                className='bg-blue-500 text-white mt-4 p-2 rounded hover:bg-blue-600'
              >
                Chọn dịch vụ này
              </button>
            </div>
          </div>
        ))
      ) : (
        <div className=' text-red-500 text-xl mt-5'>
          Gia sư này chưa có lớp riêng.
        </div>
      )}
      {selectedClassInfo && (
        <ModalChooseService
          isOpen={isModalOpen}
          onClose={closeModal}
          onConfirm={confirmSelection}
          selectedDate={selectedDate}
          selectedTimeSlots={selectedTimeSlot || ''}
          classInfo={selectedClassInfo.serviceDetails}
        />
      )}
      <Pagination
        totalItems={classData.length}
        itemsPerPage={itemsPerPage}
        currentPage={currentPage}
        onPageChange={handlePageChange}
      />
    </div>
  )
}
