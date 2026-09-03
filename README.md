![build](https://github.com/planara/planara-notifications/actions/workflows/build.yml/badge.svg)
![release](https://github.com/planara/planara-notifications/actions/workflows/release.yml/badge.svg)
![publish-k3s](https://github.com/planara/planara-notifications/actions/workflows/publish-k3s.yml/badge.svg?branch=main)
![version](https://img.shields.io/github/v/tag/planara/planara-notifications?sort=semver)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](http://makeapullrequest.com)

# Planara Notifications

Сервис доставки уведомлений.
Получает события из Kafka, сохраняет задачи на доставку и обрабатывает их в фоновом режиме.
На текущем этапе сервис поддерживает отправку email-уведомлений через SMTP. HTML-шаблоны поставляются отдельно из репозитория `planara-email-templates` и рендерятся с помощью Scriban.
