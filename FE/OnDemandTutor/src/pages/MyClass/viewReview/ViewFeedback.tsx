import { faStar, faTimes } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'

import { useQuery } from '@tanstack/react-query'
import { tutorApi } from '../../../api/tutor.api'

import userImage from '../../../assets/img/user.svg'
import { ViewReviewRequestBody } from '../../../types/request.type'

interface ViewFeedbackProps {
  onClose: () => void
  idTutor: string
}

export default function ViewFeedback({ onClose, idTutor }: ViewFeedbackProps) {
  console.log(idTutor)

  const { data: ReviewList } = useQuery({
    queryKey: ['Account'],
    queryFn: () => tutorApi.getReview(idTutor),
    enabled: !!idTutor
  })

  return (
    <div className='fixed inset-0 flex items-center justify-center bg-black bg-opacity-50 z-50 '>
      <div className='bg-slate-100 p-6 rounded-lg shadow-lg w-11/12 h-[70%]  max-w-3xl relative  '>
        <button
          onClick={onClose}
          className='absolute top-3 right-4 text-gray-600 hover:text-gray-900'
        >
          <FontAwesomeIcon icon={faTimes} size='lg' />
        </button>

        {/* Hiển thị danh sách đánh giá */}
        <div className='h-full overflow-y-auto'>
          {ReviewList && ReviewList.length > 0 ? (
            ReviewList.map((review: ViewReviewRequestBody, index: number) => (
              <div
                key={index}
                className='flex mb-4 ml-4 rounded-lg p-4 m-2 duration-700 hover:shadow-md hover:shadow-black'
              >
                {/* Avatar */}
                <div className='flex flex-col items-center'>
                  <div className='h-20 w-20'>
                    <img
                      src={review.user.avatar || userImage}
                      className='h-full w-full rounded-full object-cover'
                      alt='Avatar'
                    />
                  </div>
                </div>

                {/* Nội dung đánh giá */}
                <div className='flex flex-col justify-start ml-4'>
                  <div className='font-bold text-[10px] mb-2 ml-0 text-left'>
                    {review.user.fullName}
                  </div>
                  <div className='flex items-center mb-2'>
                    {[1, 2, 3, 4, 5].map((starIndex) => (
                      <FontAwesomeIcon
                        key={starIndex}
                        icon={faStar}
                        className={`cursor-pointer transition-colors duration-200 w-3 ${
                          starIndex <= review.rating
                            ? 'text-yellow-400'
                            : 'text-gray-400'
                        }`}
                      />
                    ))}
                  </div>
                  <span className='text-left text-gray-500 text-[12px]'>
                    Phản hồi
                  </span>
                  <div className='text-black p-2'>{review.feedback}</div>
                </div>
              </div>
            ))
          ) : (
            <p className='text-center text-gray-600 font-bold mt-4'>
              Gia sư này chưa được đánh giá.
            </p>
          )}
        </div>
      </div>
    </div>
  )
}
