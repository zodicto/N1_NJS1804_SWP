import { keepPreviousData, useQuery } from '@tanstack/react-query'
import { Select } from 'antd'
import Search from 'antd/es/transfer/search'
import { useContext, useEffect, useState } from 'react'
import { FaSearch } from 'react-icons/fa'
import { studentApi } from '../../../api/student.api'
import Pagination from '../../../components/Pagination'
import { Request } from '../../../types/request.type'

import { AppContext } from '../../../context/app.context'
import RequestComponents from '../components/RequestComponents'

const options1 = [
  { label: 'Lọc theo thời gian' },
  { value: 'apple', label: 'Thời gian' },
  { value: 'banana', label: 'Môn' },
  { value: 'cherry', label: '=))' }
]
const options2 = [
  { label: 'Phương thức' },
  { value: 'apple', label: 'Học trực tiếp' },
  { value: 'banana', label: 'Học online' },
  { value: 'cherry', label: 'Học theo tiếng' }
]
const options3 = [
  { label: 'Môn' },
  { value: 'apple', label: 'Toán' },
  { value: 'banana', label: 'Tiếng Việt' },
  { value: 'cherry', label: 'Tiếng Anh' }
]

export function RequestStudentPending() {
  const { profile } = useContext(AppContext)

  const { data: RequestData, refetch } = useQuery<Request[]>({
    queryKey: ['Request'],
    queryFn: () => studentApi.pendingRequest(profile?.id as string),
    enabled: !!profile?.id,
    placeholderData: keepPreviousData
  })

  useEffect(() => {
    refetch()
  }, [RequestData, profile])

  const [currentPage, setCurrentPage] = useState(1)
  const itemsPerPage = 4

  const handlePageChange = (page: number) => {
    setCurrentPage(page)
  }

  const items = RequestData || []
  const startIndex = (currentPage - 1) * itemsPerPage
  const currentItems = items.slice(startIndex, startIndex + itemsPerPage)

  const [selectedOption1, setSelectedOption1] = useState(null)
  const [selectedOption2, setSelectedOption2] = useState(null)
  const [selectedOption3, setSelectedOption3] = useState(null)

  const handleSelectChange = (option: any) => {
    setSelectedOption1(option)
  }
  const handleSelectChange2 = (option: any) => {
    setSelectedOption2(option)
  }
  const handleSelectChange3 = (option: any) => {
    setSelectedOption3(option)
  }

  console.log('profile', profile)
  console.log('RequestData', RequestData)

  return (
    <div className='bg-gray-200 w-full p-3'>
      <div className='m-3'>
        <Search />
      </div>
      <div className='flex '>
        <div className='text-left flex justify-between p-3 w-11/12'>
          <Select
            value={selectedOption1}
            onChange={handleSelectChange}
            options={options1}
            placeholder='Lọc theo'
            className='w-[200px]'
          />
          <Select
            value={selectedOption2}
            onChange={handleSelectChange2}
            options={options2}
            placeholder='Lọc theo'
            className='w-[200px]'
          />
          <Select
            value={selectedOption3}
            onChange={handleSelectChange3}
            options={options3}
            placeholder='Lọc theo'
            className='w-[200px]'
          />
        </div>
        <div className=' ml-5 h-14 flex-nowrap content-center hover:scale-150 transition-transform duration-300 cursor-pointer'>
          <div className=''>
            <FaSearch />
          </div>
        </div>
      </div>

      <div className='p-3'>
        <div className='flex justify-between font-bold mr-20 ml-32'>
          <div>Thông tin đơn</div>
          <div>Tình Trạng</div>
        </div>
        <div className='pt-5 bg-transparent rounded-lg around w-full'>
          {currentItems.map((request, key) => (
            <RequestComponents
              key={request.idRequest}
              request={request}
              refetch={refetch}
            />
          ))}
        </div>
      </div>
      <Pagination
        totalItems={items.length}
        itemsPerPage={itemsPerPage}
        currentPage={currentPage}
        onPageChange={handlePageChange}
      />
    </div>
  )
}
