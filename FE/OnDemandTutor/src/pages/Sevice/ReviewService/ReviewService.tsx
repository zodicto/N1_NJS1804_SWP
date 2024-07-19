import { yupResolver } from '@hookform/resolvers/yup'
import { useMutation } from '@tanstack/react-query'
import { useContext, useState } from 'react'
import { useForm } from 'react-hook-form'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faStar } from '@fortawesome/free-solid-svg-icons'

import { studentApi } from '../../../api/student.api'
import { AppContext } from '../../../context/app.context'
import Button from '../../../components/Button'
import Input from '../../../components/Input'
import { reviewTT, ReviewTT } from '../../../utils/rules'
import { toast } from 'react-toastify'
import { ReviewServiceType } from '../../../types/request.type'

interface Props {
  idBooking: string
}

type FormData = ReviewTT
const reviewSchema = reviewTT

export default function ReviewService({ idBooking }: Props) {
  const { profile } = useContext(AppContext)
  const idUser = profile?.id as string

  const [rating, setRating] = useState<number>(0)
  const [hoverRating, setHoverRating] = useState<number>(0)

  // Initialize useForm with the validation schema
  const {
    register,
    handleSubmit,
    formState: { errors },
    reset
  } = useForm<FormData>({
    resolver: yupResolver(reviewSchema),
    defaultValues: {
      rating: 0
    }
  })

  const createReviewMutation = useMutation({
    mutationFn:studentApi.CreateServiceReview
  })

  const onSubmit = handleSubmit((data: FormData) => {
    const formData: ReviewServiceType = {
      ...data,
      rating: rating,
      feedBack: data.feedback,
      idBooking,
      idUser
    }

    console.log('formData', formData)

    createReviewMutation.mutate(formData, {
      onSuccess: (data) => {
        toast.success(data.data.message)
        reset() // Reset form after successful submission
      },
      onError: (error) => {
        toast.error(error.message)
      }
    })
  })

  const handleMouseEnter = (index: number) => {
    setHoverRating(index)
  }

  const handleMouseLeave = () => {
    setHoverRating(0)
  }

  const handleClick = (index: number) => {
    setRating(index)
  }

  return (
    <div className='container border-2 h-full rounded-2xl w-full p-3 hover:shadow-xl hover:shadow-black transition-shadow duration-700'>
      <form onSubmit={onSubmit}>
        <h2 className='mt-4'>Phản hồi giảng dạy</h2>

        <div>
          <label htmlFor='feedback'>Phản hồi của bạn</label>
          <Input
            name='feedback'
            type='text'
            register={register}
            className='rounded-md p-2 w-full'
            errorMessage={errors.feedback?.message}
          />
        </div>

        {/*  đánh sao */}
        <div className='mt-4'>
          <label htmlFor=''>Đánh giá</label>
          <div className='flex justify-center items-center gap-1'>
            {[1, 2, 3, 4, 5].map((index) => (
              <FontAwesomeIcon
                key={index}
                icon={faStar}
                className={`cursor-pointer transition-colors duration-200 ${
                  (hoverRating || rating) >= index
                    ? 'text-yellow-400'
                    : 'text-gray-400'
                }`}
                onMouseEnter={() => handleMouseEnter(index)}
                onMouseLeave={handleMouseLeave}
                onClick={() => handleClick(index)}
              />
            ))}
          </div>
        </div>

        <Button
          type='submit'
          className='w-full rounded-xl text-center bg-pink-300 py-3 px-2 uppercase text-white text-sm hover:bg-pink-600 flex justify-center items-center'
        >
          Gửi
        </Button>
      </form>
    </div>
  )
}
