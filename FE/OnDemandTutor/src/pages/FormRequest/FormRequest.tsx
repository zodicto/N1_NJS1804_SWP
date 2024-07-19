import { yupResolver } from '@hookform/resolvers/yup'
import { useContext, useState } from 'react'
import { Controller, useForm } from 'react-hook-form'
import { Requestchema, requestSchema } from '../../utils/rules'
import Input from '../../components/Input'
import InputNumber from '../../components/InputNumber'
import DateOfWeek from '../../components/DayOfWeek/DayOfWeek'
import { studentApi } from '../../api/student.api'
import { useMutation } from '@tanstack/react-query'
import { RequestBody } from '../../types/user.request.type'
import { toast } from 'react-toastify'
import { AppContext } from '../../context/app.context'
import { Request } from '../../types/request.type'

type FormData = Pick<
  Requestchema,
  | 'class'
  | 'learningMethod'
  | 'description'
  | 'price'
  | 'subject'
  | 'timeEnd'
  | 'timeStart'
  | 'timeTable'
  | 'title'
  | 'totalSessions'
>

interface FormRequestProps {
  onClose: () => void
  idRequest?: string
  refetch?: (() => void) | undefined
  request?: Request
}

export default function FormRequest({
  onClose,
  idRequest,
  refetch,
  request
}: FormRequestProps) {
  const { profile } = useContext(AppContext)
  const {
    control,
    handleSubmit,
    formState: { errors },
    setValue,
    trigger,
    register
  } = useForm<FormData>({
    resolver: yupResolver(requestSchema),
    defaultValues: {
      title: request?.title,
      subject: request?.subject,
      class: (request?.class ?? '10') as '10' | '11' | '12',
      learningMethod: request?.learningMethod as
        | 'Dạy trực tiếp(offline)'
        | 'Dạy trực tuyến (online)',
      price: request?.price,
      timeStart: request?.timeStart,
      timeEnd: request?.timeEnd,
      description: request?.description,
      timeTable: request?.timeTable
    }
  })

  const [showConfirmation, setShowConfirmation] = useState<boolean>(false)
  const ReqMutation = useMutation({
    mutationFn: (body: RequestBody) =>
      studentApi.createRequest(profile?.id as string, body)
  })

  const UpdateReqMutation = useMutation({
    mutationFn: studentApi.updateRequest
  })

  const onSubmit = handleSubmit((data) => {
    const { timeTable } = data
    const totalSessions = timeTable.split(',').length

    const newData = {
      ...data,
      totalSessions: totalSessions
    }
    console.log(newData)

    if (idRequest) {
      UpdateReqMutation.mutate(
        { idReq: idRequest, dataUpdate: newData },
        {
          onSuccess: (response) => {
            if (refetch) {
              refetch(), toast.success(response.data.message), onClose()
            }
          },
          onError: (error) => {
            toast.error(error.message)
          }
        }
      )
    } else {
      ReqMutation.mutate(newData, {
        onSuccess: () => {
          toast.success('Yêu cầu của bạn đang chờ để xét duyệt')
          onClose()
        },
        onError: (error) => {
          toast.error(error.message)
        }
      })
    }
  })

  const handleConfirmCancel = () => {
    setShowConfirmation(false)
    onClose()
  }

  return (
    <div className='fixed inset-0 flex items-center justify-center bg-black bg-opacity-50 z-50'>
      <div className='p-6 h-[45rem] w-full max-w-4xl'>
        <form
          onSubmit={onSubmit}
          className='w-full h-auto space-y-4 border-2 rounded-xl p-4 bg-white shadow-black shadow-lg'
        >
          <div>
            <h1 className='text-lg text-red-500'>
              {idRequest ? 'Chỉnh sửa yêu cầu' : 'Đăng ký yêu cầu'}
            </h1>
          </div>
          <div className='grid grid-cols-2 gap-4'>
            <div className='flex flex-col'>
              <label className='block text-sm font-medium'>Tựa đề</label>
              <Input
                name='title'
                type='text'
                placeholder='Nhập tựa đề'
                register={register}
                classNameInput='w-full p-3 outline-none border border-gray-300 focus:border-gray-500 focus:shadow-sm rounded-xl'
                classNameError='text-red-600 mt-1 text-[0.75rem]'
                errorMessage={errors.title?.message}
              />
            </div>
            {/* --------------- */}
            <div className='flex flex-col'>
              <label className='block text-sm font-medium'>Môn học</label>
              <select
                {...register('subject')}
                className='w-full p-3 outline-none border border-gray-300 focus:border-gray-500 focus:shadow-sm rounded-xl text-center'
              >
                <option value=''>Chọn môn học</option>
                <option value='Ngữ văn'>Ngữ văn</option>
                <option value='Toán học'>Toán học</option>
                <option value='Vật lý'>Vật lý</option>
                <option value='Hóa học'>Hóa học</option>
                <option value='Sinh học'>Sinh học</option>
                <option value='Lịch sử'>Lịch sử</option>
                <option value='Địa lý'>Địa lý</option>
                <option value='Giáo dục công dân'>Giáo dục công dân</option>
                <option value='Ngoại ngữ'>Ngoại ngữ</option>
                <option value='Tin học'>Tin học</option>
              </select>
              {errors.subject && (
                <p className='text-red-600 mt-1 text-[0.75rem]'>
                  {errors.subject.message}
                </p>
              )}
            </div>
            {/* -------------- */}
            <div className='flex flex-col'>
              <label className='block text-lg font-medium'>
                Phương thức học
              </label>
              <select
                {...register('learningMethod')}
                className='w-full p-3 outline-none border border-gray-300 focus:border-gray-500 focus:shadow-sm rounded-xl'
              >
                <option value=''>Chọn phương thức học</option>
                <option value='Dạy trực tiếp(offline)'>
                  Dạy trực tiếp (offline)
                </option>
                <option value='Dạy trực tuyến (online)'>
                  Dạy trực tuyến (online)
                </option>
              </select>
              {errors.learningMethod && (
                <p className='text-red-600 mt-1 text-[0.75rem]'>
                  {errors.learningMethod.message}
                </p>
              )}
            </div>
            {/* ----------- */}
            <div className='flex flex-col'>
              <label className='block text-lg font-medium'>Giá</label>
              <Controller
                control={control}
                name='price'
                render={({ field }) => (
                  <InputNumber
                    inputType='price'
                    placeholder='Nhập học phí'
                    classNameInput='w-full h-[2.75rem] rounded-xl border-2 p-3'
                    classNameError='text-red-600 mt-1 text-[0.75rem] text-center'
                    errorMessage={errors.price?.message}
                    {...field}
                  />
                )}
              />
            </div>
            <div className='flex flex-col'>
              <label className='block text-lg font-medium'>Chọn lớp</label>
              <select
                {...register('class')}
                className='w-full p-3 outline-none border border-gray-300 focus:border-gray-500 focus:shadow-sm rounded-xl'
              >
                <option value='10'>Lớp 10</option>
                <option value='11'>Lớp 11</option>
                <option value='12'>Lớp 12</option>
              </select>
              {errors.class && (
                <p className='text-red-600 mt-1 text-center'>
                  {errors.class.message}
                </p>
              )}
            </div>

            <div className='flex flex-col'>
              <label>Thời khóa biểu</label>
              <Controller
                control={control}
                name='timeTable'
                render={({ field }) => (
                  <DateOfWeek
                    value={field.value}
                    onChange={field.onChange}
                    errorMessage={errors.timeTable?.message}
                  />
                )}
              />
            </div>
            <div className='flex flex-col'>
              <label className='block text-lg font-medium'>
                Thời gian bắt đầu
              </label>
              <Input
                name='timeStart'
                type='time'
                register={register}
                classNameInput='w-full p-3 outline-none border border-gray-300 focus:border-gray-500 focus:shadow-sm rounded-xl'
                classNameError='text-red-600 mt-1 text-[0.75rem]'
                errorMessage={errors.timeStart?.message}
                onChange={() => {
                  trigger('timeEnd')
                }}
              />
            </div>
            <div className='flex flex-col'>
              <label className='block text-lg font-medium'>
                Thời gian kết thúc
              </label>
              <Input
                name='timeEnd'
                type='time'
                register={register}
                classNameInput='w-full p-3 outline-none border border-gray-300 focus:border-gray-500 focus:shadow-sm rounded-xl'
                classNameError='text-red-600 mt-1 text-[0.75rem]'
                errorMessage={errors.timeEnd?.message}
              />
            </div>
            <div className='col-span-2'>
              <label className='block text-lg font-medium'>Mô tả</label>
              <textarea
                {...register('description')}
                className='w-full p-3 outline-none border border-gray-300 focus:border-gray-500 focus:shadow-sm rounded-xl'
                rows={4}
              />
              {errors.description && (
                <p className='text-red-600 mt-1 text-[0.75rem]'>
                  {errors.description.message}
                </p>
              )}
            </div>
            <div className='col-span-2 flex justify-between'>
              <div className='w-[49%]'>
                <button
                  type='submit'
                  className='w-full p-3 bg-pink-500 text-white rounded-lg hover:bg-pink-300 focus:outline-none'
                >
                  Nộp
                </button>
              </div>
              <div className='w-[49%]'>
                <button
                  type='button'
                  onClick={() => setShowConfirmation(true)}
                  className='w-full p-3 bg-black text-white rounded-lg hover:bg-gray-500 hover:shadow-lg focus:outline-none'
                >
                  Hủy
                </button>
              </div>
            </div>
          </div>
        </form>
        {showConfirmation && (
          <div className='fixed top-0 left-0 w-full h-full bg-black bg-opacity-50 flex justify-center items-center'>
            <div className='bg-white p-4 rounded-lg'>
              <p className='text-lg mb-4'>Bạn có chắc chắn muốn hủy không?</p>
              <div className='flex justify-end'>
                <button
                  className='px-4 py-2 bg-red-500 text-white rounded-lg mr-2 hover:bg-red-600'
                  role='submit'
                  onClick={() => {
                    setShowConfirmation(false)
                  }}
                >
                  Hủy
                </button>
                <button
                  className='px-4 py-2 bg-green-500 text-white rounded-lg hover:bg-green-600'
                  type='button'
                  onClick={handleConfirmCancel}
                >
                  Đồng ý
                </button>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  )
}
