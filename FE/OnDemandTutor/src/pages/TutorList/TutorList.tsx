import {
  faPersonHalfDress,
  faSchool,
  faStar,
  faStarHalfAlt,
  faUserGraduate
} from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { useQuery } from '@tanstack/react-query'

import { useState } from 'react'
import { adminAPI } from '../../api/admin.api'
import userAvatar from '../../assets/img/user.svg'
import Pagination from '../../components/Pagination'
import Popup from '../../components/Popup/Popup'
import TutorRender from '../../components/TutorRender/TutorRender'
import { AdminTutorProfile } from '../../types/tutor.type'
import ViewFeedback from '../MyClass/viewReview'

export default function TutorList() {
  const [isPopupVisible, setIsPopupVisible] = useState(false)
  const [isViewFeedbackVisible, setIsViewFeedbackVisible] = useState(false)
  const [currentTutor, setCurrentTutor] = useState<AdminTutorProfile | null>(
    null
  )
  const [searchText, setSearchText] = useState('')
  const [selectedSubject, setSelectedSubject] = useState('')
  const [selectedTutorId, setSelectedTutorId] = useState<string | null>(null)
  const [currentPage, setCurrentPage] = useState(1)
  const itemsPerPage = 4

  const { data: TutorListProfile, refetch } = useQuery({
    queryKey: ['tutorList'],
    queryFn: () => adminAPI.getTutorList()
  })

  const handleClosePopup = () => {
    setIsPopupVisible(false)
    setCurrentTutor(null)
  }

  const handleCloseFeedback = () => {
    setIsViewFeedbackVisible(false)
    setSelectedTutorId(null)
  }

  const handleItemClick = (tutor: AdminTutorProfile) => {
    setCurrentTutor(tutor)
    setIsPopupVisible(true)
  }

  const handleViewFeedback = (idTutor: string) => {
    setSelectedTutorId(idTutor)
    setIsViewFeedbackVisible(true)
  }

  const handleSubjectChange = (value: string) => {
    setSelectedSubject(value)
  }

  const filteredTutors = TutorListProfile?.filter(
    (tutor: AdminTutorProfile) =>
      tutor.fullName.toLowerCase().includes(searchText.toLowerCase()) &&
      (selectedSubject === '' ||
        tutor.subjects.toLowerCase().includes(selectedSubject.toLowerCase()))
  )

  const indexOfLastItem = currentPage * itemsPerPage
  const indexOfFirstItem = indexOfLastItem - itemsPerPage
  const currentItems = filteredTutors?.slice(indexOfFirstItem, indexOfLastItem)

  const handlePageChange = (pageNumber: number) => {
    setCurrentPage(pageNumber)
  }

  return (
    <div className='contaier min-h-96 justify-center items-center flex'>
      <div>
        <div className='flex'>
          <div className='w-full'>
            <input
              type='text'
              placeholder='Tìm kiếm gia sư theo tên'
              className='border border-gray-300 p-2 rounded-lg w-full'
              value={searchText}
              onChange={(e) => setSearchText(e.target.value)}
            />
          </div>
          <div className='ml-2'>
            <select
              value={selectedSubject}
              onChange={(e) => setSelectedSubject(e.target.value)}
              className='border border-gray-300 p-2 rounded-lg'
            >
              <option value=''>Tất cả môn học</option>
              {Array.from(
                new Set(TutorListProfile?.map((item) => item.subjects))
              ).map((subject) => (
                <option key={subject} value={subject}>
                  {subject}
                </option>
              ))}
            </select>
          </div>
        </div>

        {filteredTutors?.length === 0 ? (
          <p className='text-center text-gray-600 font-bold mt-4'>
            Hiện tại danh sách các gia sư chưa có.
          </p>
        ) : (
          currentItems?.map((tutor, index) => (
            <div
              key={index}
              className='w-[1230px] rounded-3xl border-3 bg-transparent border-2 h-auto mx-auto my-5 px-5 hover:shadow-lg hover:shadow-gray-900'
            >
              <div className='col-span-12 flex'>
                <div
                  className='col-span-3'
                  onClick={() => handleItemClick(tutor)}
                >
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
                          <FontAwesomeIcon
                            icon={faSchool}
                            className='pt-2 h-6'
                          />
                          <span className='pl-2 pt-1'>
                            Môn học: {tutor.subjects}
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
                        onClick={() => handleItemClick(tutor)}
                        className='pt-3'
                      >
                        Xem chi tiết
                      </button>
                    </div>
                    <div className='capitalize border-[3px] rounded-lg w-full h-10 bg-white hover:bg-slate-200 mt-2 mx-auto'>
                      <button
                        onClick={() => handleViewFeedback(tutor.id)}
                        className=' py-2'
                      >
                        Xem phản hồi
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          ))
        )}

        {isPopupVisible && currentTutor && (
          <Popup
            handleHidden={handleClosePopup}
            renderPopover={<TutorRender currentTutor={currentTutor} />}
          />
        )}

        {/* Feedback popup */}
        {isViewFeedbackVisible && selectedTutorId && (
          <ViewFeedback
            idTutor={selectedTutorId}
            onClose={handleCloseFeedback}
          />
        )}

        <Pagination
          currentPage={currentPage}
          itemsPerPage={itemsPerPage}
          totalItems={filteredTutors?.length || 0}
          onPageChange={handlePageChange}
        />
      </div>
    </div>
  )
}
