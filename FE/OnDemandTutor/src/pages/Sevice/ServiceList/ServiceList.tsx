import { useMutation, useQuery } from '@tanstack/react-query'
import { useEffect, useState } from 'react'
import { toast } from 'react-toastify'
import { moderatorApi } from '../../../api/moderator.api'
import { studentApi } from '../../../api/student.api'
import Pagination from '../../../components/Pagination'
import { roles } from '../../../constant/roles'
import { ServiceTutor } from '../../../types/request.type'
import { User } from '../../../types/user.type'
import { getProfileFromLS } from '../../../utils/auth'
import ModalChooseService from '../components/ModalChooseService'
import ScheduleFormToChoose from '../components/ScheduleFormToChose'

export default function ServiceList() {
  const user: User = getProfileFromLS()

  const [selectedClassIndex, setSelectedClassIndex] = useState<number | null>(
    null
  )
  const [selectedDate, setSelectedDate] = useState<string>('')
  const [selectedTimeSlot, setSelectedTimeSlot] = useState<string | null>('')
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false)
  const [classData, setClassData] = useState<ServiceTutor[]>([])
  const [currentPage, setCurrentPage] = useState(1)
  const [selectedSubject, setSelectedSubject] = useState<string>('')
  const [searchServiceId, setSearchServiceId] = useState<string>('') // State for search service ID
  const itemsPerPage = 4

  const { data: classService, refetch } = useQuery<ServiceTutor[]>({
    queryKey: ['allServices'],
    queryFn: () =>
      studentApi.GetAllService().then((response) => response.data.data)
  })

  const items = classService || []

  useEffect(() => {
    if (classService) {
      setClassData(classService)
    }
  }, [classService])

  const handleDateChange = (classIndex: number, date: string) => {
    setSelectedClassIndex(classIndex)
    setSelectedDate(date)
    setSelectedTimeSlot(null)
  }

  const handleTimeSlotChange = (timeSlot: string) => {
    setSelectedTimeSlot(timeSlot)
  }

  const handleSubmit = () => {
    if (selectedClassIndex === null) return

    const newClassData = [...classData]
    const scheduleIndex = newClassData[selectedClassIndex].schedule.findIndex(
      (s) => s.date === selectedDate
    )
    if (scheduleIndex === -1) return

    newClassData[selectedClassIndex].schedule[scheduleIndex].timeSlots =
      selectedTimeSlot ? [selectedTimeSlot] : []
    setClassData(newClassData)
    setIsModalOpen(false)
    refetch() // Refetch the data
  }

  const getDayOfWeek = (dateString: string) => {
    const date = new Date(dateString)
    return date.toLocaleDateString('vi-VN', { weekday: 'long' })
  }

  const openModal = () => {
    setIsModalOpen(true)
  }

  const closeModal = () => {
    setIsModalOpen(false)
  }

  const onConfirm = () => {
    handleSubmit()
  }

  // Toggle description logic
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

  // Pagination logic
  const totalItems = classData.length
  const startIndex = (currentPage - 1) * itemsPerPage
  const endIndex = startIndex + itemsPerPage
  const paginatedData = classData.slice(startIndex, endIndex)

  const handlePageChange = (page: number) => {
    setCurrentPage(page)
  }

  // Delete service logic
  const handleDeleteService = (idSer: string) => {
    const isConfirmed = window.confirm('Bạn có chắc chắn muốn xóa không?')
    if (isConfirmed) {
      deleteMutation.mutate(idSer)
    }
  }

  const deleteMutation = useMutation({
    mutationFn: (idSer: string) => moderatorApi.deleteService(idSer),
    onSuccess: () => {
      toast.success('Yêu cầu đã bị xóa')
      refetch() // Refresh the request list after deletion
    }
  })

  // Filter data by service ID logic
  const filteredData = searchServiceId
    ? classData.filter((item) =>
        item.idService?.toLowerCase().includes(searchServiceId.toLowerCase())
      )
    : classData

  return (
    <div className='p-4'>
      <div className='flex justify-center mb-4'>
        <select
          value={selectedSubject}
          onChange={(e) => setSelectedSubject(e.target.value)}
          className='border border-gray-300 p-2 rounded-lg'
        >
          <option value=''>Tất cả môn học</option>
          {Array.from(new Set(classService?.map((item) => item.subject))).map(
            (subject) => (
              <option key={subject} value={subject}>
                {subject}
              </option>
            )
          )}
        </select>
        <input
          type='text'
          value={searchServiceId}
          onChange={(e) => setSearchServiceId(e.target.value)}
          placeholder='Nhập mã dịch vụ...'
          className='ml-4 border border-gray-300 p-2 rounded-lg'
        />
      </div>
      {filteredData.length === 0 && (
        <div className='text-center text-gray-600 font-bold mt-4'>
          Hiện tại chưa có dịch vụ nào
        </div>
      )}
      {filteredData.map((item, classIndex) => (
        <div
          key={classIndex}
          className='w-full bg-transparent border-2 rounded-2xl grid grid-cols-2 hover:shadow-xl transition-shadow translate-x-4 duration-700 shadow-md gap-2 bg-white'
        >
          <div className='col-span-1 p-4'>
            <h2 className='text-xl font-bold mb-2'>{item.title}</h2>
            <div className='pl-10 p-7 bg-gray-100 rounded-lg shadow-md'>
              <div className='text-left h-full mx-auto'>
                <p className='mb-2'>
                  <strong className='text-pink-500'>Tiêu đề:</strong>{' '}
                  <span
                    className={item.title ? 'text-gray-800' : 'text-red-500'}
                  >
                    {item.title ? item.title : 'Title trống'}
                  </span>
                </p>
                <p className='mb-2'>
                  <strong className='text-blue-700'>Mã dịch vụ:</strong>{' '}
                  <span className='text-gray-800'>{item.idService}</span>
                </p>
                <p className='mb-2'>
                  <strong className='text-blue-700'>Môn học:</strong>{' '}
                  <span className='text-gray-800'>{item.subject}</span>
                </p>
                <p className='mb-2'>
                  <strong className='text-blue-700'>Lớp:</strong>{' '}
                  <span className='text-gray-800'>{item.class}</span>
                </p>
                <p className='mb-2'>
                  <strong className='text-blue-700'>Phương thức học:</strong>{' '}
                  <span className='text-gray-800'>{item.learningMethod}</span>
                </p>
                <p className='mb-2'>
                  <strong className='text-blue-700'>Giá trên một giờ:</strong>{' '}
                  <span className='text-green-700'>
                    {item.pricePerHour} VNĐ
                  </span>
                </p>
                <p>
                  <strong className='text-blue-700'>Mô tả:</strong>{' '}
                  <span className='text-gray-800'>
                    {renderDescription(item.description)}
                  </span>
                  {item.description.length > 100 && (
                    <button
                      onClick={toggleDescription}
                      className='text-blue-500 ml-2 hover:underline'
                    >
                      {isDescriptionExpanded ? 'Ẩn bớt' : 'Xem thêm'}
                    </button>
                  )}
                </p>
              </div>
              {user?.roles.toLowerCase() === roles.moderator && (
                <div className='flex justify-end mt-2'>
                  <button
                    onClick={() => handleDeleteService(item.idService!)}
                    className='bg-red-700 text-white px-4 py-2 rounded-md w-full hover:bg-slate-600 hover:shadow-xl hover:shadow-black mb-2'
                  >
                    Xóa
                  </button>
                </div>
              )}
            </div>
          </div>
          <div className='col-span-1 p-4'>
            <p>
              <strong>Thời gian:</strong>
            </p>
            <ScheduleFormToChoose
              schedule={item.schedule}
              classIndex={classIndex}
              selectedDate={
                selectedClassIndex === classIndex ? selectedDate : ''
              }
              handleDateChange={handleDateChange}
              handleTimeSlotChange={handleTimeSlotChange}
              getDayOfWeek={getDayOfWeek}
            />
            {selectedClassIndex === classIndex &&
              selectedTimeSlot &&
              item.idAccountTutor !== user.id && (
                <button
                  type='button'
                  onClick={openModal}
                  className='bg-blue-500 hover:bg-blue-600 text-white font-bold py-2 px-4 rounded mt-4'
                >
                  Xác nhận chọn
                </button>
              )}
            {selectedClassIndex === classIndex &&
              selectedTimeSlot &&
              item.idAccountTutor === user.id && (
                <button
                  type='button'
                  onClick={openModal}
                  className='bg-slate-500 hover:bg-slate-400 text-white font-bold py-2 px-4 rounded mt-4 '
                >
                  Lớp học này là của bạn
                </button>
              )}
          </div>
        </div>
      ))}
      <ModalChooseService
        isOpen={isModalOpen}
        onClose={closeModal}
        onConfirm={onConfirm}
        selectedDate={selectedDate}
        selectedTimeSlots={selectedTimeSlot || ''}
        classInfo={
          selectedClassIndex !== null ? classData[selectedClassIndex] : null
        }
      />
      <Pagination
        totalItems={totalItems}
        itemsPerPage={itemsPerPage}
        currentPage={currentPage}
        onPageChange={handlePageChange}
      />
    </div>
  )
}
