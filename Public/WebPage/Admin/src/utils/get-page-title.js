import defaultSettings from '@/settings'

const title = defaultSettings.title || '电商Demo管理端'

export default function getPageTitle(pageTitle) {
  if (pageTitle) {
    return `${pageTitle} - ${title}`
  }
  return `${title}`
}
