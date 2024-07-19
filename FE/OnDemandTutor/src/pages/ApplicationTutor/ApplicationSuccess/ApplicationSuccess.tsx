import { useContext } from 'react'
import TutorApplicationComponent from '../components'
import NavTutorApplication from '../Nav'
import { AppContext } from '../../../context/app.context'
import { keepPreviousData, useQuery } from '@tanstack/react-query'
import { tutorApi } from '../../../api/tutor.api'
import { statusReq } from '../../../constant/status.Req'

export default function ApplicationSuccess() {
  const { profile } = useContext(AppContext)

  const { data, refetch } = useQuery({
    queryKey: ['Account', profile?.id as string],
    queryFn: () => tutorApi.getRegisterTutor(profile?.id as string),
    enabled: !!profile?.id,
    placeholderData: keepPreviousData
  })
  const tutorRes = data?.data
  console.log('data nè', data?.data)

  return (
    <div className='w-4/5'>
      <NavTutorApplication />
      <div className='border-2 shadow-xl'>
        {tutorRes?.status.toLowerCase() === statusReq.approved ? (
          <TutorApplicationComponent tutor={tutorRes} refetch={refetch} />
        ) : (
          <div className='text-slate-500 text-lg'>
            Hiện tại bạn chưa có đơn nào
          </div>
        )}
      </div>
    </div>
  )
}
