import { useEffect, useState } from 'react'
import TurorMenu from '../AdminMenu/TutorMenu'
import { Button, Modal, Table, TableColumnsType } from 'antd'
import Search from 'antd/es/transfer/search'
import { useMutation, useQuery } from '@tanstack/react-query'
import { AdminTutorProfile } from '../../../../types/tutor.type'
import { toast } from 'react-toastify'
import { adminAPI } from '../../../../api/admin.api'

function AdminListTutor() {
  const { data: TutorList, refetch } = useQuery<AdminTutorProfile[]>({
    queryKey: ['TutorList'],
    queryFn: () => adminAPI.getTutorList()
  })

  console.log('TutorList', TutorList)

  const removeMutation = useMutation({
    mutationFn: (idAccount: string) => adminAPI.deleteAccount(idAccount),
    onSuccess: () => {
      toast.success('Đã xóa tài khoản')
      refetch()
      setVisible(false)
    }
  })

  useEffect(() => {
    if (TutorList) {
      console.log('TutorList', TutorList)
    }
  }, [TutorList])

  const [selectedRecord, setSelectedRecord] =
    useState<AdminTutorProfile | null>(null)
  const [visible, setVisible] = useState(false)
  const [isDetails, setIsDetails] = useState(false)

  const onChange = () => {}

  const showImg = (record: AdminTutorProfile) => {
    setIsDetails(false)
    setVisible(true)
    setSelectedRecord(record)
  }

  const showDetail = (record: AdminTutorProfile) => {
    setSelectedRecord(record)
    setVisible(true)
    setIsDetails(true) // Set isDetails to true when showing details
  }

  const handleCancel = () => {
    setVisible(false)
    setSelectedRecord(null)
  }

  const handleDelete = () => {
    if (selectedRecord) {
      removeMutation.mutate(selectedRecord.id)
      console.log('id của thằng request nè ', selectedRecord.id)
    }
  }

  const columns: TableColumnsType<AdminTutorProfile> = [
    {
      title: 'Tên',
      dataIndex: 'fullName',
      defaultSortOrder: 'descend',
      onFilter: (value, record) =>
        record.fullName.indexOf(value as string) === 0,
      sorter: (a, b) => a.fullName.length - b.fullName.length,
      width: 200,
      fixed: 'left'
    },
    {
      title: 'Ngày sinh',
      dataIndex: 'date_of_birth',
      defaultSortOrder: 'descend',
      width: 200,
      sorter: (a, b) =>
        new Date(a.date_of_birth).getTime() -
        new Date(b.date_of_birth).getTime()
    },
    {
      title: 'Giới Tính',
      dataIndex: 'gender',
      sorter: (a, b) => parseInt(a.gender) - parseInt(b.gender),
      width: 200
    },
    {
      title: 'Tên Môn Học',
      dataIndex: 'subject',
      defaultSortOrder: 'descend',
      width: 200
    },
    {
      title: 'Kinh Nghiệm',
      dataIndex: 'experience',
      defaultSortOrder: 'descend',
      width: 200,
      sorter: (a, b) => a.experience - b.experience
    },
    {
      title: 'Ảnh Bằng Cấp(Chứng chỉ)',
      dataIndex: 'qualifications',
      defaultSortOrder: 'descend',
      width: 150,
      render: (text: string, record: AdminTutorProfile) => (
        <div className='flex gap-1'>
          {record.qualifications.map((qualification, index) => (
            <button
              key={index}
              className='border border-red-500 rounded-lg hover:bg-red-500 active:bg-red-700'
              onClick={() => showImg(record)}
            >
              Xem bằng {index + 1}
            </button>
          ))}
        </div>
      ),
      fixed: 'right'
    },
    {
      title: 'Chi tiết',
      dataIndex: 'detail',
      className: 'TextAlign',
      width: 100,
      render: (text: string, record: AdminTutorProfile) => (
        <div className='flex gap-1'>
          <button
            className='p-1 border border-red-500 rounded-lg hover:bg-red-500 active:bg-red-700'
            onClick={() => showDetail(record)}
          >
            Chi tiết
          </button>
        </div>
      ),
      fixed: 'right'
    }
  ]

  return (
    <>
      <div>
        <div className='text-left'>Quản lý gia sư</div>
        <TurorMenu list='list' con='' rej='' />
        <div className='text-left shadow-sm shadow-black border-4 pt-5 h-[629px] rounded-t-xl mt-6'>
          <div className='mb-5'>
            <Search />
          </div>
          <Table
            columns={columns}
            dataSource={TutorList}
            pagination={{ pageSize: 6 }}
            onChange={onChange}
            showSorterTooltip={{ target: 'sorter-icon' }}
            scroll={{ x: 1300, y: 400 }}
          />
        </div>
        <Modal
          title='Chi tiết'
          visible={visible}
          onCancel={handleCancel}
          footer={[
            <Button key='back' onClick={handleDelete}>
              Xóa
            </Button>
          ]}
        >
          {selectedRecord && (
            <div>
              <p>Tên: {selectedRecord.fullName}</p>
              <p>
                Ngày sinh: {selectedRecord.date_of_birth || 'Chưa cập nhật'}
              </p>
              <p>Giới tính: {selectedRecord.gender || 'Chưa cập nhật'}</p>
              <p>Môn học: {selectedRecord.subjects || 'Chưa cập nhật'}</p>
              <p>
                Kinh nghiệm dạy: {selectedRecord.experience || 'Chưa cập nhật'}
              </p>
              <p>
                Kỹ năng đặc biệt:{' '}
                {selectedRecord.specializedSkills || 'Chưa cập nhật'}
              </p>
              {isDetails ? (
                selectedRecord.qualifications.map((qualification, index) => (
                  <div key={index}>
                    <p>
                      Bằng cấp(Chứng chỉ) {index + 1}:{' '}
                      {qualification.type || 'Chưa cập nhật'}
                    </p>
                    <p>
                      Tên bằng cấp {index + 1}:{' '}
                      {qualification.qualificationName || 'Chưa cập nhật'}
                    </p>
                    <p>
                      Ảnh bằng cấp(Chứng chỉ) {index + 1}:{' '}
                      {qualification.img ? (
                        <img src={qualification.img} alt={`Ảnh ${index + 1}`} />
                      ) : (
                        'Chưa cập nhật'
                      )}
                    </p>
                  </div>
                ))
              ) : (
                <p>
                  Ảnh:{' '}
                  {selectedRecord.qualifications &&
                  selectedRecord.qualifications.length > 0 ? (
                    <img src={selectedRecord.qualifications[0].img} alt='Ảnh' />
                  ) : (
                    'Chưa cập nhật'
                  )}
                </p>
              )}
            </div>
          )}
        </Modal>
      </div>
    </>
  )
}

export default AdminListTutor
