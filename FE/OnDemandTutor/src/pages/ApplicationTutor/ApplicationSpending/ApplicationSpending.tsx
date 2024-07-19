import { keepPreviousData, useQuery } from '@tanstack/react-query'
import TutorApplicationComponent from '../components'
import NavTutorApplication from '../Nav'
import { tutorApi } from '../../../api/tutor.api'
import { useContext } from 'react'
import { AppContext } from '../../../context/app.context'
import { statusReq } from '../../../constant/status.Req'

export default function ApplicationSpending() {
  const { profile } = useContext(AppContext)

  const { data, refetch } = useQuery({
    queryKey: ['Account', profile?.id as string],
    queryFn: () => tutorApi.getRegisterTutor(profile?.id as string),
    enabled: !!profile?.id,
    placeholderData: keepPreviousData
  })

  const tutorRes = data?.data

  console.log('data nè', tutorRes)

  return (
    <div className='w-4/5'>
      <NavTutorApplication />
      <div className='border-2 shadow-xl'>
        {tutorRes?.status.toLowerCase() === statusReq.pending ? (
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
