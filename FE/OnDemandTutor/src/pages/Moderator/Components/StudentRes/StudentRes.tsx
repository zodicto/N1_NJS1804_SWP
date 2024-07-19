import React, { useEffect, useState } from 'react'
import { Modal, Table, Button, Input } from 'antd'
import { useMutation, useQuery } from '@tanstack/react-query'
import { moderatorApi } from '../../../../api/moderator.api'
import { toast } from 'react-toastify'
import { RequestModerator } from '../../../../types/request.type'

export default function StudentRes() {
  const [rejectReason, setRejectReason] = useState('')
  const [selectedRecord, setSelectedRecord] = useState<RequestModerator | null>(
    null
  )
  const [visible, setVisible] = useState(false)
  const [rejectVisible, setRejectVisible] = useState(false)

  const { data: RequestData, refetch } = useQuery<RequestModerator[]>({
    queryKey: ['Request'],
    queryFn: () => moderatorApi.viewRequests()
  })

  const approveMutation = useMutation({
    mutationFn: (idReq: string) => moderatorApi.approvedRequest(idReq),
    onSuccess: (data) => {
      toast.success(data.data.message)
      refetch()
      setVisible(false)
    }
  })

  const rejectMutation = useMutation({
    mutationFn: ({ idReq, reason }: { idReq: string; reason: string }) =>
      moderatorApi.rejectRequest({ idReq, reason }),
    onSuccess: (data) => {
      toast.success(data.data.message)
      refetch()
      setRejectVisible(false)
    }
  })

  useEffect(() => {
    if (approveMutation.isSuccess) {
      refetch()
      setVisible(false)
    }
    if (rejectMutation.isSuccess) {
      refetch()
      setRejectVisible(false)
    }
  }, [approveMutation.isSuccess, rejectMutation.isSuccess])

  const handleApprove = () => {
    if (selectedRecord) {
      approveMutation.mutate(selectedRecord.idRequest)
    }
  }

  const handleReject = () => {
    if (selectedRecord && rejectReason) {
      rejectMutation.mutate({
        idReq: selectedRecord.idRequest,
        reason: rejectReason
      })
    }
  }

  const handleDelete = (idReq: string) => {
    deleteMutation.mutate(idReq)
  }

  const deleteMutation = useMutation({
    mutationFn: (idReq: string) => moderatorApi.deleteRequest(idReq),
    onSuccess: (data) => {
      toast.success(data.data.message)
      refetch() // Gọi lại API để cập nhật lại danh sách yêu cầu
    }
  })

  const columns: {
    title: string
    dataIndex: string
    width: number
    fixed?: 'left' | 'right'
    render?: (_: any, record: RequestModerator) => React.ReactNode
  }[] = [
    {
      title: 'Tên Học Sinh',
      dataIndex: 'fullName',
      width: 200,
      fixed: 'left'
    },
    {
      title: 'Môn',
      dataIndex: 'subject',
      width: 200
    },
    {
      title: 'Tựa Đề',
      dataIndex: 'title',
      width: 200
    },
    {
      title: 'Giá',
      dataIndex: 'price',
      width: 150
    },
    {
      title: 'Phương thức học',
      dataIndex: 'learningMethod',
      width: 150
    },
    {
      title: 'Ngày',
      dataIndex: 'timeTable',
      width: 150
    },
    {
      title: 'Số buổi',
      dataIndex: 'totalSessions',
      width: 150
    },
    {
      title: 'Giờ bắt đầu',
      dataIndex: 'timeStart',
      width: 150
    },
    {
      title: 'Giờ kết thúc',
      dataIndex: 'timeEnd',
      width: 150
    },
    {
      title: '',
      dataIndex: 'detail',
      fixed: 'right',
      width: 200,
      render: (_: any, record: RequestModerator) => (
        <div className='flex gap-1'>
          <Button
            className='border w-2/3 border-blue-400 rounded-lg hover:bg-blue-600 active:bg-blue-500'
            onClick={() => showDetail(record.idRequest)}
          >
            Chi tiết
          </Button>
          <Button
            className='border w-1/3 border-red-500 rounded-lg hover:bg-red-500 active:bg-red-700'
            onClick={() => showRejectForm(record)}
          >
            Từ chối
          </Button>
        </div>
      )
    }
  ]

  const showDetail = (id: string) => {
    const record = RequestData?.find((item) => item.idRequest === id) || null
    setSelectedRecord(record)
    setVisible(true)
  }

  const showRejectForm = (record: RequestModerator) => {
    setSelectedRecord(record)
    setRejectVisible(true)
  }

  const handleCancel = () => {
    setVisible(false)
    setRejectVisible(false)
    setSelectedRecord(null)
    setRejectReason('')
  }

  return (
    <>
      <div className='text-left shadow-2xl shadow-black border-4 pt-5 h-[629px] rounded-t-xl mt-6'>
        <Table
          columns={columns}
          dataSource={RequestData}
          pagination={{ pageSize: 10 }}
          scroll={{ x: 1300, y: 400 }}
        />
        <Modal
          title='Chi tiết'
          visible={visible}
          onCancel={handleCancel}
          footer={[
            <Button key='approve' onClick={handleApprove}>
              Xác nhận
            </Button>,
            <Button
              key='reject'
              onClick={() => showRejectForm(selectedRecord!)}
            >
              Từ chối
            </Button>
          ]}
        >
          {selectedRecord && (
            <div className='details'>
              <p className='detail-item'>
                <span className='font-medium text-gray-600'>Tên:</span>{' '}
                <span className='font-bold  '>{selectedRecord.fullName}</span>
              </p>
              <p className='detail-item'>
                <span className='font-medium text-gray-600'>Ngày học:</span>{' '}
                <span className='font-bold  '>{selectedRecord.timeTable}</span>
              </p>
              <p className='detail-item'>
                <span className='font-medium text-gray-600'>Số ngày học:</span>{' '}
                <span className='font-bold text-yellow-600'>
                  {selectedRecord.totalSessions}
                </span>
              </p>
              <p className='detail-item'>
                <span className='font-medium text-gray-600'>Thời gian:</span>{' '}
                <span className='font-bold text-purple-600'>
                  từ {selectedRecord.timeStart} tới {selectedRecord.timeEnd}
                </span>
              </p>
              <p className='detail-item'>
                <span className='font-medium text-gray-600'>Môn học:</span>{' '}
                <span className='font-bold text-green-600'>
                  {selectedRecord.subject}
                </span>
              </p>
              <p className='detail-item'>
                <span className='font-medium text-gray-600'>Học phí:</span>{' '}
                <span className='font-bold text-red-600'>
                  {selectedRecord.price}
                </span>
              </p>
            </div>
          )}
        </Modal>
        <Modal
          title='Từ chối yêu cầu'
          visible={rejectVisible}
          onCancel={handleCancel}
          footer={null}
        >
          {selectedRecord && (
            <div className='mt-4'>
              <textarea
                placeholder='Nhập lý do từ chối'
                value={rejectReason}
                onChange={(e) => setRejectReason(e.target.value)}
                className='w-full p-2 border rounded'
              />
              <Button className='mt-2' type='primary' onClick={handleReject}>
                Xác nhận từ chối
              </Button>
            </div>
          )}
        </Modal>
      </div>
    </>
  )
}
