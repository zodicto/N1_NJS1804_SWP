import Search from 'antd/es/transfer/search'
import ModMenu from '../ModMenu/ModMenu'
import { useEffect, useState } from 'react'
import { Button, Modal, Table, TableColumnsType } from 'antd'
import { moderatorApi } from '../../../../api/moderator.api'
import { useMutation, useQuery } from '@tanstack/react-query'
import { toast } from 'react-toastify'
import { TutorType } from '../../../../types/tutor.type'

export default function ModTutorResRegis() {
  const [searchText, setSearchText] = useState('') // liên quan đến giá trị input vào search
  const [selectedRecord, setSelectedRecord] = useState<TutorType | null>(null)
  const [open, setOpen] = useState(false)
  const [isDetails, setIsDetails] = useState(false)
  const [showRejectReason, setShowRejectReason] = useState(false)
  const [rejectReason, setRejectReason] = useState('')

  // Lấy danh sách yêu cầu từ API
  const { data: requestData, refetch } = useQuery({
    queryKey: ['RequestTutorReg'],
    queryFn: () => moderatorApi.getRequestTutorReg()
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
    mutationFn: ({
      tutorReqID,
      reason
    }: {
      tutorReqID: string
      reason: string
    }) => moderatorApi.rejectTutorReg({ tutorReqID, reason }),
    onSuccess: () => {
      toast.success('Yêu cầu đã bị từ chối')
      refetch() // Gọi lại API để cập nhật lại danh sách yêu cầu
      setOpen(false)
      setShowRejectReason(false)
      setRejectReason('')
    }
  })

  useEffect(() => {
    if (requestData) {
      console.log(requestData)
    }
  }, [requestData])

  const handleApprove = () => {
    if (selectedRecord) {
      approveMutation.mutate(selectedRecord.id)
    }
  }
  const handleReject = () => {
    if (selectedRecord && rejectReason.trim() !== '') {
      rejectMutation.mutate({
        tutorReqID: selectedRecord.id as string,
        reason: rejectReason as string
      })
    } else {
      toast.error('Vui lòng nhập lý do từ chối')
    }
  }
  const columns: TableColumnsType<TutorType> = [
    {
      // định nghĩa từng cột
      title: 'Tên', // tên của cột hay còn gọi là header của cột
      dataIndex: 'fullName', // xác định trường nào trong interface DataType
      width: 200,
      fixed: 'left'
    },
    {
      title: 'Ngày sinh',
      dataIndex: 'date_of_birth',
      width: 150
    },
    {
      title: 'Giới Tính',
      dataIndex: 'gender',
      width: 100
    },
    {
      title: 'Tên Môn Học',
      dataIndex: 'subject',
      width: 150
    },
    {
      title: 'Kinh Nghiệm(Năm)',
      dataIndex: 'experience',
      width: 200
    },
    {
      title: 'Tên Bằng Cấp(Chứng chỉ)',
      dataIndex: 'qualifiCationName',
      width: 200
    },
    {
      title: 'Kĩ Năng Nổi bật',
      dataIndex: 'specializedSkills',
      width: 200
    },
    {
      title: 'Ảnh bằng cấp',
      dataIndex: 'imageQualification',
      className: 'TextAlign',
      width: 120,
      fixed: 'right',
      render: (text: string, record: TutorType) => (
        <div className='flex gap-1'>
          <button
            className='border  p-1 border-blue-400 rounded-lg hover:bg-blue-600 active:bg-blue-500 w-20'
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
      render: (text: string, record: TutorType) => (
        <div className='flex gap-1'>
          <button
            className='p-1 border border-red-500 rounded-lg hover:bg-red-500 active:bg-red-700'
            onClick={() => showDetail(record)}
          >
            Chi tiết
          </button>
        </div>
      )
    }
  ]

  const onChange = () => {}

  const showDetail = (record: TutorType) => {
    setSelectedRecord(record)
    setOpen(true)
    setIsDetails(true)
  }
  const handleCancel = () => {
    setOpen(false)
    setSelectedRecord(null)
    setShowRejectReason(false)
    setRejectReason('')
  }
  const showImg = (record: TutorType) => {
    setSelectedRecord(record)
    setOpen(true)
    setIsDetails(false)
  }
  const title = isDetails ? 'Chi tiết' : 'Ảnh'
  return (
    <>
      <div>
        <div className='text-left'>Yêu cầu trở thành gia sư</div>
        <ModMenu kind='tutor' style='Option1' />
        <div className='text-left shadow-2xl shadow-black border-4 pt-5 h-[629px] rounded-t-xl mt-6'>
          <div className='mb-5'>
            <Search />
          </div>
          <Table
            className=''
            columns={columns}
            dataSource={requestData}
            pagination={{ pageSize: 10 }}
            onChange={onChange}
            showSorterTooltip={{ target: 'sorter-icon' }}
            scroll={{ x: 1300, y: 400 }}
            rowKey={(record) => record.id}
          />
          <div>
            <Modal
              title={title}
              open={open}
              onCancel={handleCancel}
              footer={[
                <Button key='approve' onClick={handleApprove}>
                  Xác nhận
                </Button>,
                <Button key='reject' onClick={() => setShowRejectReason(true)}>
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
                      <p>Kinh nghiệm dạy : {selectedRecord.experience} Năm</p>
                    </div>
                  ) : (
                    <p>
                      Ảnh :{' '}
                      <img src={selectedRecord.imageQualification} alt='ảnh' />
                    </p>
                  )}
                </div>
              )}
              {showRejectReason && (
                <div className='mt-4'>
                  <textarea
                    placeholder='Nhập lý do từ chối'
                    value={rejectReason}
                    onChange={(e) => setRejectReason(e.target.value)}
                    className='w-full p-2 border rounded'
                  />
                  <Button
                    className='mt-2'
                    type='primary'
                    onClick={handleReject}
                  >
                    Xác nhận từ chối
                  </Button>
                </div>
              )}
            </Modal>
          </div>
        </div>
      </div>
    </>
  )
}
