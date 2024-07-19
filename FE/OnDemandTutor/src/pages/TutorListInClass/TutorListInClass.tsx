import {
  faAudioDescription,
  faBook,
  faImage,
  faPersonHalfDress,
  faSchool,
  faStar,
  faStarHalfAlt,
  faUserGraduate
} from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { useMutation, useQuery } from '@tanstack/react-query'
import { useState } from 'react'
import { useParams } from 'react-router-dom'
import { toast } from 'react-toastify'
import { studentApi } from '../../api/student.api'
import Popup from '../../components/Popup/Popup'
import { TutorType } from '../../types/tutor.type'
import { SelecTutorReqBody } from '../../types/user.request.type'

import userAvatar from '../../assets/img/user.svg'
import ViewFeedback from '../MyClass/viewReview'
import Pagination from '../../components/Pagination'

export default function TutorListInClass() {
  const [isPopupVisible, setIsPopupVisible] = useState(false)
  const [currentTutor, setCurrentTutor] = useState<TutorType | null>(null)
  const [searchText, setSearchText] = useState('')
  const [isViewFeedbackVisible, setIsViewFeedbackVisible] = useState(false)
  const [selectedTutorId, setSelectedTutorId] = useState<string | null>(null)
  const [currentPage, setCurrentPage] = useState(1)
  const itemsPerPage = 4

  const { idReq: idRequestParams } = useParams()

  const { data: TutorListProfile, refetch } = useQuery({
    queryKey: ['Request', idRequestParams],
    queryFn: () =>
      studentApi.viewAllTutorsJoinRequests(idRequestParams as string),
    enabled: !!idRequestParams
  })

  const selectTutorMutation = useMutation({
    mutationFn: (body: SelecTutorReqBody) => studentApi.selectTutor(body)
  })

  const handleClosePopup = () => {
    setIsPopupVisible(false)
    setCurrentTutor(null)
  }

  const handleItemClick = (tutor: TutorType) => {
    setCurrentTutor(tutor)
    setIsPopupVisible(true)
  }

  const handleApproved = (idTutor: string) => {
    if (idRequestParams) {
      console.log('idRequestParams', idRequestParams)
      console.log('idTutor', idTutor)

      selectTutorMutation.mutate(
        { idRequest: idRequestParams, idTutor },
        {
          onSuccess: (data) => {
            toast.success(data.data.message)
            refetch()
          },
          onError: (data) => {
            toast.error(data.message)
          }
        }
      )
    } else {
      console.error('Không có id của request')
    }
  }

  const filteredTutors = TutorListProfile?.filter((tutor: TutorType) =>
    tutor.fullName.toLowerCase().includes(searchText.toLowerCase())
  )

  const handleViewFeedback = (idTutor: string) => {
    setSelectedTutorId(idTutor)
    setIsViewFeedbackVisible(true)
  }

  const handleCloseFeedback = () => {
    setIsViewFeedbackVisible(false)
  }

  const indexOfLastItem = currentPage * itemsPerPage
  const indexOfFirstItem = indexOfLastItem - itemsPerPage
  const currentItems = filteredTutors?.slice(indexOfFirstItem, indexOfLastItem)

  const handlePageChange = (pageNumber: number) => {
    setCurrentPage(pageNumber)
  }

  return (
    <div>
      <input
        type='text'
        placeholder='Tìm kiếm gia sư'
        className='border border-gray-300 p-2 rounded-lg w-1/3'
        value={searchText}
        onChange={(e) => setSearchText(e.target.value)}
      />

      {currentItems?.map((tutor, index) => (
        <div
          key={index}
          className='w-[1230px] rounded-3xl border-3 bg-transparent border-2 h-auto mx-auto my-5 px-5 hover:shadow-lg hover:shadow-gray-900'
        >
          <div className='col-span-12 flex'>
            <div className='col-span-3' onClick={() => handleItemClick(tutor)}>
              <div className='mx-8 my-5'>
                <div className='w-[13rem] h-[15rem]'>
                  <img
                    src={tutor.avatar ? tutor.avatar : userAvatar}
                    alt='ảnh đại diện'
                    className='w-full h-full'
                  />
                </div>
              </div>
            </div>

            <div
              className='col-span-5 mx-4 my-5'
              onClick={() => handleItemClick(tutor)}
            >
              <div className='w-[45rem] h-full py-2'>
                <div className='justify-start flex pl-2'>
                  <div>
                    <h1 className='text-2xl font-bold text-start'>
                      Tên: {tutor.fullName}
                    </h1>
                    <div className='text-lg justify-start flex pl-1 pt-2'>
                      <FontAwesomeIcon
                        icon={faPersonHalfDress}
                        className='pt-2 h-6'
                      />
                      <span className='pl-2 pt-1'>
                        Giới tính: {tutor.gender}
                      </span>
                    </div>
                    <div className='text-lg justify-start flex pl-1 pt-2'>
                      <FontAwesomeIcon icon={faSchool} className='pt-2 h-6' />
                      <span className='pl-2 pt-1'>
                        Môn học: {tutor.subject}
                      </span>
                    </div>
                    <div className='text-lg justify-start flex pl-1 pt-2'>
                      <FontAwesomeIcon
                        icon={faUserGraduate}
                        className='pt-2 h-6'
                      />
                      <span className='pl-2 pt-1'>
                        Kinh nghiệm: {tutor.experience}
                      </span>
                    </div>
                    <div className='text-lg justify-start flex pl-1 pt-2'>
                      <FontAwesomeIcon icon={faStar} className='pt-2 h-6' />
                      <span className='pl-2 pt-1'>
                        Kỹ năng đặc biệt: {tutor.specializedSkills}
                      </span>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <div className='col-span-4 w-full h-full'>
              <div className='h-full w-full flex justify-end'>
                <div className='mt-4'>
                  <div className='flex justify-center items-center gap-1'>
                    {[1, 2, 3, 4, 5].map((index) => (
                      <FontAwesomeIcon
                        key={index}
                        icon={
                          tutor.rating >= index
                            ? faStar
                            : tutor.rating >= index - 0.5
                            ? faStarHalfAlt
                            : faStar
                        }
                        className={`cursor-pointer transition-colors duration-200 ${
                          tutor.rating >= index - 0.5
                            ? 'text-yellow-400'
                            : 'text-gray-400'
                        }`}
                      />
                    ))}
                  </div>
                </div>
              </div>

              <div className='w-full h-full px-auto mx-auto pt-32'>
                <div className='rounded-lg w-full h-10 bg-pink-400 hover:opacity-80'>
                  <button
                    onClick={() => handleApproved(tutor.id)}
                    className='pt-3'
                  >
                    Chấp nhận
                  </button>
                </div>
                <div className='capitalize border-[3px] rounded-lg w-full h-10 bg-white hover:bg-slate-200 mt-2 mx-auto flex justify-center items-center'>
                  <button
                    onClick={() => handleViewFeedback(tutor.id)}
                    className='py-2 text-center w-full'
                  >
                    Xem phản hồi
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      ))}

      {isPopupVisible && currentTutor && (
        <Popup
          handleHidden={handleClosePopup}
          renderPopover={
            <div className='overflow-y-auto p-4'>
              <div className='flex justify-center py-4'>
                <img
                  src={currentTutor.avatar ? currentTutor.avatar : userAvatar}
                  alt='gia sư'
                  className='w-32 h-32 rounded-full'
                />
              </div>
              <div className='mx-4 my-5'>
                <div className='py-2'>
                  <div className='justify-start flex pl-2'>
                    <div>
                      <h1 className='text-2xl font-bold text-start'>
                        {currentTutor.fullName}
                      </h1>
                      <div className='text-lg flex pl-1 pt-2'>
                        <FontAwesomeIcon
                          icon={faPersonHalfDress}
                          className='pt-2 h-6'
                        />
                        <span className='pl-2 pt-1'>{currentTutor.gender}</span>
                      </div>
                      <div className='text-lg flex pl-1 pt-2'>
                        <FontAwesomeIcon icon={faSchool} className='pt-2 h-6' />
                        <span className='pl-2 pt-1'>
                          {currentTutor.subject}
                        </span>
                      </div>
                      <div className='text-lg flex pl-1 pt-2'>
                        <FontAwesomeIcon
                          icon={faUserGraduate}
                          className='pt-2 h-6'
                        />
                        <span className='pl-2 pt-1'>
                          {currentTutor.experience}
                        </span>
                      </div>
                      <div className='text-lg flex pl-1 pt-2'>
                        <FontAwesomeIcon icon={faStar} className='pt-2 h-6' />
                        <span className='pl-2 pt-1'>
                          {currentTutor.specializedSkills}
                        </span>
                      </div>
                      <div className='text-lg flex pl-1 pt-2'>
                        <FontAwesomeIcon
                          icon={faAudioDescription}
                          className='pt-2 h-6'
                        />
                        <span className='pl-2 pt-1'>
                          {currentTutor.introduction}
                        </span>
                      </div>
                      <div className='text-lg flex pl-1 pt-2'>
                        <FontAwesomeIcon icon={faBook} className='pt-2 h-6' />
                        <span className='pl-2 pt-1'>{currentTutor.type}</span>
                      </div>
                      <div className='text-lg flex pl-1 pt-2'>
                        <FontAwesomeIcon icon={faImage} className='pt-2 h-6' />
                        <span className='pl-2 pt-1'>
                          {currentTutor.qualifiCationName}
                        </span>
                      </div>
                      <div className='text-lg flex pl-1 pt-2'>
                        <FontAwesomeIcon icon={faStar} className='pt-2 h-6' />
                        <span className='pl-2 pt-1'>{currentTutor.rating}</span>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          }
        />
      )}

      {isViewFeedbackVisible && selectedTutorId && (
        <ViewFeedback idTutor={selectedTutorId} onClose={handleCloseFeedback} />
      )}

      <Pagination
        currentPage={currentPage}
        itemsPerPage={itemsPerPage}
        totalItems={filteredTutors?.length || 0}
        onPageChange={handlePageChange}
      />
    </div>
  )
}
