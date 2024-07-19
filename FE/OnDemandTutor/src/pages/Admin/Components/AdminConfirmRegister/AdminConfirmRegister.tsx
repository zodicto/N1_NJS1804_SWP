import Search from 'antd/es/transfer/search'
import { useEffect, useState } from 'react'
import { Button, Modal, Table, TableColumnsType } from 'antd'
import { moderatorApi } from '../../../../api/moderator.api'
import { useMutation, useQuery } from '@tanstack/react-query'
import { toast } from 'react-toastify'
import { AdminTutorType, TutorType } from '../../../../types/tutor.type'
import TurorMenu from '../AdminMenu/TutorMenu'
import { adminAPI } from '../../../../api/admin.api'

export default function ModTutorResRegis() {
  const [searchText, setSearchText] = useState('') // liên quan đến giá trị input vào search
  const [selectedRecord, setSelectedRecord] = useState<AdminTutorType | null>(null)
  const [open, setOpen] = useState(false)
  const [isDetails, setIsDetails] = useState(false)
  // Lấy danh sách yêu cầu từ API
  const { data: requestData, refetch } = useQuery<any>({
    queryKey: ['RequestTutorReg'],
    queryFn: () => adminAPI.getRequestTutorReg()
  })

  // Khởi tạo các mutation cho việc phê duyệt và từ chối yêu cầu
  const approveMutation = useMutation({
    mutationFn: (idReg: string) => moderatorApi.approvedTutorReg(idReg),
    onSuccess: () => {
      toast.success('Yêu cầu đã được phê duyệt')
      refetch() // Gọi lại API để cập nhật lại danh sách yêu cầu
      setOpen(false)
    }
  })
  const rejectMutation = useMutation({
    mutationFn: (idReg: string) => moderatorApi.rejectTutorReg(idReg),
    onSuccess: () => {
      toast.success('Yêu cầu đã bị từ chối')
      refetch() // Gọi lại API để cập nhật lại danh sách yêu cầu
      setOpen(false)
    }
  })
  useEffect(() => {
    if (requestData) {
      console.log(requestData)
    }
  }, [requestData])

  const handleApprove = () => {
    if (selectedRecord) {
      console.log('selectedRecord nè', selectedRecord)
      console.log('id selectedRecord nè', selectedRecord.id)
      approveMutation.mutate(selectedRecord.id)
    }
  }
  const handleReject = () => {
    if (selectedRecord) {
      console.log('selectedRecord nè', selectedRecord)
      console.log('id selectedRecord nè', selectedRecord.id)
      rejectMutation.mutate(selectedRecord.id)
    }
  }
  const columns: TableColumnsType<AdminTutorType> = [
    {
      // định nghĩa từng cột
      title: 'Tên', // tên của cột hay còn gọi là header của cột
      dataIndex: 'fullName', // xác định trường nào trong interface DataType
      //defaultSortOrder: "descend",
      //onFilter: (value, record) => record.FullName.indexOf(value as string) === 0,
      //sorter: (a, b) => a.FullName.length - b.FullName.length,
      width: 200,
      fixed: 'left'
    },
    {
      title: 'Ngày sinh',
      dataIndex: 'date_of_birth',
      width: 150
      //defaultSortOrder: "descend",
      //sorter: (a, b) => new Date(a.Date_Of_Birth).getTime() - new Date(b.Date_Of_Birth).getTime()
    },
    {
      title: 'Giới Tính',
      dataIndex: 'gender',
      //sorter: (a, b) => parseInt(a.Gender) - parseInt(b.Gender),
      width: 100
    },
    {
      title: 'Tên Môn Học',
      dataIndex: 'subject',
      //defaultSortOrder: "descend",
      width: 150
      //sorter: (a, b) => parseInt(a.SubjectName) - parseInt(b.SubjectName),
    },
    {
      title: 'Kinh Nghiệm(Năm)',
      dataIndex: 'experience',
      width: 200
      //defaultSortOrder: "descend",
      //sorter: (a, b) => (a.Experience - b.Experience)
    },
    {
      title: 'Tên Bằng Cấp(Chứng chỉ)',
      dataIndex: 'qualifiCationName',
      //defaultSortOrder: "descend"
      width: 200
    },
    {
      title: 'Kĩ Năng Nổi bật',
      dataIndex: 'specializedSkills',
      width: 200
    },
    {
      title: 'Ảnh bằng cấp',
      dataIndex: 'imageQualifiCation',
      className: 'TextAlign',
      width: 120,
      fixed: 'right',
      render: (text: string, record: AdminTutorType) => (
        <div className='flex gap-1'>
          <button
            className='p-1 border border-red-500 rounded-lg hover:bg-red-500 active:bg-red-700'
            onClick={() => showImg(record)}
          >
            Xem Bằng
          </button>
        </div>
      )
    },
    {
      title: 'Xem duyệt',
      dataIndex: 'action',
      className: 'TextAlign',
      fixed: 'right',
      width: 150,
      render: (text: string, record: AdminTutorType) => (
        <div className='flex gap-1'>
          <button
            className='p-1 border border-red-500 rounded-lg hover:bg-red-500 active:bg-red-700'
            onClick={() => {
              console.log('record',record)
              showDetail(record)}}
          >
            Chi tiết
          </button>
        </div>
      )
    }
  ]

  const onChange = () => {}

  const showDetail = (record: AdminTutorType) => {
    setSelectedRecord(record)
    
    setOpen(true)
    setIsDetails(true)
  }
  const handleCancel = () => {
    setOpen(false)
    setSelectedRecord(null)
  }
  const showImg = (record: AdminTutorType) => {
    setSelectedRecord(record)
    setOpen(true)
    setIsDetails(false)
  }
  const title = isDetails ? 'Chi tiết' : 'Ảnh'
  return (
    <>
      <div>
        <div className='text-left'>Yêu cầu trở thành gia sư</div>
        <TurorMenu
            list=""
            con="con"
            rej=""
            />
        <div className='text-left shadow-sm shadow-black border-4 pt-5 h-[629px] rounded-t-xl mt-6'>
          <div className='mb-5'>
            <Search
            // inputText={searchText}
            // placeHolder="Search đi nè"
            // setInputValue={setSearchText}
            // label="Q"
            />
          </div>
          <Table
            className=''
            columns={columns}
            dataSource={requestData}
            pagination={{ pageSize: 10 }}
            onChange={onChange}
            showSorterTooltip={{ target: 'sorter-icon' }}
            scroll={{ x: 1300, y: 400 }}
          />
          <div>
            <Modal
              title={title}
              open={open}
              onCancel={handleCancel}
              footer={[
                <Button key='back' onClick={handleApprove}>
                  Xác nhận
                </Button>,
                <Button key='back' onClick={handleReject}>
                  Từ chối
                </Button>
              ]}
            >
              {selectedRecord && (
                <div>
                  {isDetails ? (
                    <div>
                      <p>Tên : {selectedRecord.fullName}</p>
                      <p>Ngày sinh : {selectedRecord.date_of_birth}</p>
                      <p>Giới tính : {selectedRecord.gender}</p>
                      <p>Môn : {selectedRecord.subject}</p>
                      <p>Bằng cấp(Chứng chỉ) : {selectedRecord.type}</p>
                      <p>Tên bằng Cấp : {selectedRecord.qualifiCationName}</p>
                      <p>
                        Kĩ năng đặc biệt : {selectedRecord.specializedSkills}
                      </p>
                      {/* <img src={selectedRecord.imageQualification} alt="ảnh" />    // ảnh nè   */}
                      <p>Kinh nghiệm dạy : {selectedRecord.experience} Năm</p>
                      <p>Giới thiệu : {selectedRecord.introduction} </p>
                    </div>
                  ) : (
                    <div>
                      Ảnh :{' '}
                      <img src={selectedRecord.imageQualification} alt='ảnh' />
                    </div>
                  )}
                </div>
              )}
            </Modal>
          </div>
        </div>
      </div>
    </>
  )
}
